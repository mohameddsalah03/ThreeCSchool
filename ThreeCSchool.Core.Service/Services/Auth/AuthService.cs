using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ThreeCSchool.Core.Domain.Models.Data;
using ThreeCSchool.Core.Service.Abstraction.Services.Auth;
using ThreeCSchool.Core.Service.Abstraction.Services.Auth.Email;
using ThreeCSchool.Shared.DTOs.Auth;
using ThreeCSchool.Shared.DTOs.Auth.OTP;
using ThreeCSchool.Shared.DTOs.Auth.Profile;
using ThreeCSchool.Shared.Exceptions;
using ThreeCSchool.Shared.Settings;
using ValidationException = ThreeCSchool.Shared.Exceptions.ValidationException;

namespace ThreeCSchool.Core.Service.Services.Auth
{
    public class AuthService(
        UserManager<ApplicationUser> _userManager,
        SignInManager<ApplicationUser> _signInManager,
        IOptions<JwtSettings> _jwtSettings,
        IOptions<AppSettings> _appSettings,
        IEmailService _emailService) : IAuthService
    {
        private readonly JwtSettings _jwt = _jwtSettings.Value;
        private readonly AppSettings _app = _appSettings.Value;

        private static readonly HashSet<string> AllowedRoles =
            new() { "Student", "Instructor", "Organization" };

        public async Task<UserDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user is null)
                throw new UnauthorizedException("Invalid login credentials.");

            if (!user.IsActive)
                throw new UnauthorizedException("Your account has been deactivated.");

            if (!user.EmailConfirmed)
                throw new UnauthorizedException("Please verify your email first. Check your inbox for the OTP.");

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, lockoutOnFailure: true);

            if (result.IsLockedOut)
                throw new UnauthorizedException("Account is locked. Try again in 5 minutes.");
            if (!result.Succeeded)
                throw new UnauthorizedException("Invalid login credentials.");

            var (accessToken, refreshToken) = await GenerateTokensAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            return new UserDto
            {
                Id = user.Id,
                DisplayName = user.DisplayName,
                Email = user.Email!,
                UserName = user.UserName!,
                Token = accessToken,
                RefreshToken = refreshToken,
                TokenExpiry = DateTime.UtcNow.AddMinutes(_jwt.DurationInMinutes),
                Roles = roles
            };
        }

        public async Task<UserDto> LoginByPhoneAsync(LoginByPhoneDto dto)
        {
            var user = _userManager.Users.FirstOrDefault(u => u.PhoneNumber == dto.PhoneNumber);

            if (user is null)
                throw new UnauthorizedException("Invalid login credentials.");

            if (!user.IsActive)
                throw new UnauthorizedException("Your account has been deactivated.");
            
            if (!user.EmailConfirmed)
                throw new UnauthorizedException( "Please verify your email first. Check your inbox for the OTP.");
            
            var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, lockoutOnFailure: true);

            if (result.IsLockedOut)
                throw new UnauthorizedException("Account is locked. Try again in 5 minutes.");
            if (!result.Succeeded)
                throw new UnauthorizedException("Invalid login credentials.");

            var (accessToken, refreshToken) = await GenerateTokensAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            return new UserDto
            {
                Id = user.Id,
                DisplayName = user.DisplayName,
                Email = user.Email!,
                UserName = user.UserName!,
                Token = accessToken,
                RefreshToken = refreshToken,
                TokenExpiry = DateTime.UtcNow.AddMinutes(_jwt.DurationInMinutes),
                Roles = roles
            };
        }

        public async Task<RegisterResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            // 1. Validate role
            if (!AllowedRoles.Contains(registerDto.Role))
                throw new BadRequestException( $"Invalid role. Allowed: {string.Join(", ", AllowedRoles)}");

            // 2. بناء الـ User
            var userName = GenerateUserName(registerDto.Email);

            var user = new ApplicationUser
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = userName,
                PhoneNumber = registerDto.PhoneNumber,
                TimeZone = registerDto.TimeZone,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                EmailConfirmed = false  // مش متحقق لحد ما يعمل OTP
            };

            // 3. حفظ في DB مع hash الـ password
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
                throw new ValidationException(
                    "Registration failed.",
                    result.Errors.Select(e => e.Description));

            // 4. تعيين الـ Role
            await _userManager.AddToRoleAsync(user, registerDto.Role);

            // 5. إرسال OTP — مفيش Token هنا خالص
            await SendOtpToUserAsync(user);

            // 6. رجوع رسالة بس — مفيش JWT
            return new RegisterResponseDto
            {
                Email = user.Email!,
                Message = "Registration successful. Please check your email for the OTP.",
                RequiresOtpVerification = true
            };
        }
        public async Task<bool> EmailExists(string email)
            => await _userManager.FindByEmailAsync(email) is not null;

        public async Task ForgotPasswordAsync(ForgotPasswordDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user is null)
                throw new UnauthorizedException("No account found with this email.");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var resetUrl = $"{_app.FrontendUrl.TrimEnd('/')}/reset-password?email={user.Email}&token={encodedToken}";

            var body = $@"
                <h2>Reset Your Password</h2>
                <p>Hello {user.DisplayName},</p>
                <p>Click the link below to reset your password:</p>
                <a href='{resetUrl}'>Reset Password</a>
                <p>This link is valid for 1 hour only.</p>
                <p>If you did not request this, please ignore this email.</p>";

            await _emailService.SendEmailAsync(user.Email!, "Reset Your Password", body);
        }

        public async Task ResetPasswordAsync(ResetPasswordDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user is null)
                throw new BadRequestException("Invalid request.");

            try
            {
                var decodedToken = Encoding.UTF8.GetString(
                    WebEncoders.Base64UrlDecode(dto.Token));

                var result = await _userManager.ResetPasswordAsync(user, decodedToken, dto.NewPassword);

                if (!result.Succeeded)
                    throw new ValidationException(
                        "Failed to reset password.",
                        result.Errors.Select(e => e.Description));
            }
            catch (FormatException)
            {
                throw new BadRequestException("Invalid or expired token.");
            }
        }

        public async Task<UserDto> RefreshTokenAsync(RefreshTokenDto dto)
        {
            var principal = ValidateExpiredToken(dto.Token);

            var email = principal.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
                throw new UnauthorizedException("Invalid token.");

            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                throw new UnauthorizedException("Invalid token.");

            if (user.RefreshToken != dto.RefreshToken)
                throw new UnauthorizedException("Invalid refresh token.");

            if (user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                throw new UnauthorizedException("Refresh token has expired.");

            var (accessToken, newRefreshToken) = await GenerateTokensAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            return new UserDto
            {
                Id = user.Id,
                DisplayName = user.DisplayName,
                Email = user.Email!,
                UserName = user.UserName!,
                Token = accessToken,
                RefreshToken = newRefreshToken,
                TokenExpiry = DateTime.UtcNow.AddMinutes(_jwt.DurationInMinutes),
                Roles = roles
            };
        }


        #region Profile

        public async Task<ProfileDto> GetProfileAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
                throw new NotFoundException("User", userId);

            var roles = await _userManager.GetRolesAsync(user);

            return new ProfileDto
            {
                Id = user.Id,
                DisplayName = user.DisplayName,
                Email = user.Email!,
                PhoneNumber = user.PhoneNumber,
                ProfilePicture = user.ProfilePicture,
                TimeZone = user.TimeZone,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                IsEmailVerified = user.EmailConfirmed,
                Roles = roles
            };
        }

        public async Task<ProfileDto> UpdateProfileAsync(string userId, UpdateProfileDto dto)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
                throw new NotFoundException("User", userId);

            // Only update fields that were actually sent (not null)
            if (!string.IsNullOrWhiteSpace(dto.DisplayName))
                user.DisplayName = dto.DisplayName;

            if (!string.IsNullOrWhiteSpace(dto.PhoneNumber))
                user.PhoneNumber = dto.PhoneNumber;

            if (!string.IsNullOrWhiteSpace(dto.ProfilePicture))
                user.ProfilePicture = dto.ProfilePicture;

            if (!string.IsNullOrWhiteSpace(dto.TimeZone))
                user.TimeZone = dto.TimeZone;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                throw new ValidationException(
                    "Failed to update profile.",
                    result.Errors.Select(e => e.Description));

            var roles = await _userManager.GetRolesAsync(user);

            return new ProfileDto
            {
                Id = user.Id,
                DisplayName = user.DisplayName,
                Email = user.Email!,
                PhoneNumber = user.PhoneNumber,
                ProfilePicture = user.ProfilePicture,
                TimeZone = user.TimeZone,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                IsEmailVerified = user.EmailConfirmed,
                Roles = roles
            };
        }

        #endregion


        public async Task ChangePasswordAsync(string userId, ChangePasswordDto dto)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
                throw new NotFoundException("User", userId);

            // Verify the current password first
            var isCurrentPasswordValid = await _userManager
                .CheckPasswordAsync(user, dto.CurrentPassword);

            if (!isCurrentPasswordValid)
                throw new BadRequestException("Current password is incorrect.");

            var result = await _userManager.ChangePasswordAsync(
                user, dto.CurrentPassword, dto.NewPassword);

            if (!result.Succeeded)
                throw new ValidationException(
                    "Failed to change password.",
                    result.Errors.Select(e => e.Description));

            // Invalidate refresh token to force re-login on all devices
            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = null;
            await _userManager.UpdateAsync(user);
        }

        public async Task LogoutAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
                throw new NotFoundException("User", userId);

            // Invalidate the refresh token — access token will expire naturally (15 min)
            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = null;

            await _userManager.UpdateAsync(user);
        }

        #region OTP
        public async Task<UserDto> VerifyOtpAsync(VerifyOtpDto dto)
        {
            // 1. Find user
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user is null)
                throw new BadRequestException("Invalid request.");

            // 2. Already verified?
            if (user.EmailConfirmed)
                throw new BadRequestException("Email is already verified.");

            // 3. OTP موجود؟
            if (string.IsNullOrWhiteSpace(user.OtpCode))
                throw new BadRequestException(
                    "No OTP found. Please request a new one.");

            // 4. OTP منتهيش؟
            if (user.OtpExpiry is null || user.OtpExpiry < DateTime.UtcNow)
                throw new BadRequestException(
                    "OTP has expired. Please request a new one.");

            // 5. OTP صح؟
            if (user.OtpCode != dto.OtpCode)
                throw new BadRequestException("Invalid OTP code.");

            // 6. Confirm Email + Clear OTP
            user.EmailConfirmed = true;
            user.OtpCode = null;
            user.OtpExpiry = null;
            await _userManager.UpdateAsync(user);

            // 7. دلوقتي بس بنولد الـ Token
            var (accessToken, refreshToken) = await GenerateTokensAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            return new UserDto
            {
                Id = user.Id,
                DisplayName = user.DisplayName,
                Email = user.Email!,
                UserName = user.UserName!,
                Token = accessToken,
                RefreshToken = refreshToken,
                TokenExpiry = DateTime.UtcNow.AddMinutes(_jwt.DurationInMinutes),
                Roles = roles
            };

        }
        public async Task ResendOtpAsync(ResendOtpDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user is null)
                throw new BadRequestException("Invalid request.");

            if (user.EmailConfirmed)
                throw new BadRequestException("Email is already verified.");

            await SendOtpToUserAsync(user);
        }
        #endregion


        #region Private Helpers

        private async Task<(string accessToken, string refreshToken)> GenerateTokensAsync(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id),
                new(ClaimTypes.Name, user.UserName!),
                new(ClaimTypes.Email, user.Email!)
            };

            var roles = await _userManager.GetRolesAsync(user);
            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwt.DurationInMinutes),
                signingCredentials: creds);

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

            var refreshToken = GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwt.RefreshTokenDurationInDays);
            await _userManager.UpdateAsync(user);

            return (accessToken, refreshToken);
        }

        private static string GenerateRefreshToken()
        {
            var bytes = new byte[64];
            using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }

        private ClaimsPrincipal ValidateExpiredToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            return handler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key)),
                ValidateIssuer = true,
                ValidIssuer = _jwt.Issuer,
                ValidateAudience = true,
                ValidAudience = _jwt.Audience,
                ValidateLifetime = false,
                ClockSkew = TimeSpan.Zero
            }, out _);
        }

        private static string GenerateUserName(string email)
        {
            var local = email.Split('@')[0];
            var suffix = Guid.NewGuid().ToString("N")[..6];
            return $"{local}_{suffix}".ToLower();
        }

        // Generate & Send OTP
        private async Task SendOtpToUserAsync(ApplicationUser user)
        {
            // Generate 6-digit OTP
            var otp = new Random().Next(100000, 999999).ToString();

            // Store in DB — expires in 10 minutes
            user.OtpCode = otp;
            user.OtpExpiry = DateTime.UtcNow.AddMinutes(10);
            await _userManager.UpdateAsync(user);

            // Send via email
            var body = $@"
                        <h2>Email Verification — 3C Coding School</h2>
                        <p>Hello {user.DisplayName},</p>
                        <p>Your verification code is:</p>
                        <h1 style='letter-spacing:8px; color:#007BFF;'>{otp}</h1>
                        <p>This code is valid for <strong>10 minutes</strong>.</p>
                        <p>If you did not request this, please ignore this email.</p>";

            await _emailService.SendEmailAsync(
                user.Email!,
                "Your 3C School Verification Code",
                body);
        }


        #endregion
    }
}
