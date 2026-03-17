using ThreeCSchool.Shared.DTOs.Auth;
using ThreeCSchool.Shared.DTOs.Auth.OTP;
using ThreeCSchool.Shared.DTOs.Auth.Profile;

namespace ThreeCSchool.Core.Service.Abstraction.Services.Auth
{
    public interface IAuthService
    {
        Task<UserDto> LoginAsync(LoginDto loginDto);
        Task<UserDto> LoginByPhoneAsync(LoginByPhoneDto loginByPhoneDto);
        Task<UserDto> RegisterAsync(RegisterDto registerDto);
        Task<bool> EmailExists(string email);
        Task ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto);
        Task ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
        Task<UserDto> RefreshTokenAsync(RefreshTokenDto refreshTokenDto);


        // ── New ───────────────────────────────────────────────
        Task<ProfileDto> GetProfileAsync(string userId);
        Task<ProfileDto> UpdateProfileAsync(string userId, UpdateProfileDto dto);
        Task ChangePasswordAsync(string userId, ChangePasswordDto dto);
        Task LogoutAsync(string userId);
        Task VerifyOtpAsync(VerifyOtpDto dto);
        Task ResendOtpAsync(ResendOtpDto dto);

    }
}