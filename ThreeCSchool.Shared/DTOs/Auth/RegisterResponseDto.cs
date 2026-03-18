namespace ThreeCSchool.Shared.DTOs.Auth
{
    
    public class RegisterResponseDto
    {
        public string Email { get; set; } = null!;
        public string Message { get; set; } = null!;
        public bool RequiresOtpVerification { get; set; } = true;
    }
}