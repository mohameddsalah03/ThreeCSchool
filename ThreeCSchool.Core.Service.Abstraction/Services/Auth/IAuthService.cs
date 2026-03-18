using ThreeCSchool.Shared.DTOs.Auth;
using ThreeCSchool.Shared.DTOs.Auth.OTP;
using ThreeCSchool.Shared.DTOs.Auth.Profile;

namespace ThreeCSchool.Core.Service.Abstraction.Services.Auth
{
    public interface IAuthService
    {
        // OTP
        Task<UserDto> VerifyOtpAsync(VerifyOtpDto dto);
        Task ResendOtpAsync(ResendOtpDto dto);

        // 
        Task<RegisterResponseDto> RegisterAsync(RegisterDto registerDto);

        Task<UserDto> LoginAsync(LoginDto loginDto);

        Task<UserDto> LoginByPhoneAsync(LoginByPhoneDto dto);

        Task<bool> EmailExists(string email);

        Task ForgotPasswordAsync(ForgotPasswordDto dto);

        Task ResetPasswordAsync(ResetPasswordDto dto);

        Task<UserDto> RefreshTokenAsync(RefreshTokenDto dto);

        Task ChangePasswordAsync(string userId, ChangePasswordDto dto);
        
        Task LogoutAsync(string userId);

        // Profile
        Task<ProfileDto> GetProfileAsync(string userId);

        Task<ProfileDto> UpdateProfileAsync(string userId, UpdateProfileDto dto);
    }
}