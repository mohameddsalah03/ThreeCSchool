using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ThreeCSchool.APIs.Controllers.Controllers.Base;
using ThreeCSchool.Core.Service.Abstraction.Services.Auth;
using ThreeCSchool.Shared.DTOs.Auth;
using ThreeCSchool.Shared.DTOs.Auth.OTP;
using ThreeCSchool.Shared.DTOs.Auth.Profile;

namespace ThreeCSchool.APIs.Controllers.Controllers.Account
{
    public class AccountController(IAuthService _authService) : BaseApiController
    {
         
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register([FromBody] RegisterDto dto)
        {
            var result = await _authService.RegisterAsync(dto);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login([FromBody] LoginDto dto)
        {
            var result = await _authService.LoginAsync(dto);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("login/phone")]
        public async Task<ActionResult<UserDto>> LoginByPhone([FromBody] LoginByPhoneDto dto)
        {
            var result = await _authService.LoginByPhoneAsync(dto);
            return Ok(result);
        }

        
        [AllowAnonymous]
        [HttpGet("email-exists")]
        public async Task<ActionResult<bool>> EmailExists([FromQuery] string email)
        {
            var result = await _authService.EmailExists(email);
            return Ok(result);
        }

       
        [AllowAnonymous]
        [HttpPost("forgot-password")]
        public async Task<ActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            await _authService.ForgotPasswordAsync(dto);
            return Ok(new
            {
                message = "If this email is registered, a reset link has been sent."
            });
        }

        
        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            await _authService.ResetPasswordAsync(dto);
            return Ok(new { message = "Password has been reset successfully." });
        }

        
        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public async Task<ActionResult<UserDto>> RefreshToken([FromBody] RefreshTokenDto dto)
        {
            var result = await _authService.RefreshTokenAsync(dto);
            return Ok(result);
        }

    
        [AllowAnonymous]
        [HttpPost("verify-otp")]
        public async Task<ActionResult> VerifyOtp([FromBody] VerifyOtpDto dto)
        {
            await _authService.VerifyOtpAsync(dto);
            return Ok(new { message = "Email verified successfully. You can now log in." });
        }

        
        [AllowAnonymous]
        [HttpPost("resend-otp")]
        public async Task<ActionResult> ResendOtp([FromBody] ResendOtpDto dto)
        {
            await _authService.ResendOtpAsync(dto);
            return Ok(new { message = "A new OTP has been sent to your email." });
        }

       
        [Authorize]
        [HttpGet("profile")]
        public async Task<ActionResult<ProfileDto>> GetProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var result = await _authService.GetProfileAsync(userId);
            return Ok(result);
        }

        
        [Authorize]
        [HttpPut("profile")]
        public async Task<ActionResult<ProfileDto>> UpdateProfile([FromBody] UpdateProfileDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var result = await _authService.UpdateProfileAsync(userId, dto);
            return Ok(result);
        }

        
        [Authorize]
        [HttpPut("change-password")]
        public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            await _authService.ChangePasswordAsync(userId, dto);
            return Ok(new
            {
                message = "Password changed successfully. Please log in again on all devices."
            });
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<ActionResult> Logout()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            await _authService.LogoutAsync(userId);
            return Ok(new { message = "Logged out successfully." });
        }
    }
}
