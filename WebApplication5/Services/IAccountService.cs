using WebApplication5.Models;

namespace WebApplication5.Servies
{
    public interface IAccountService
    {
        Task RegisterAsync(RegisterDto dto);
        Task<string> LoginAsync(LoginDto dto);
        Task SendOtpAsync(string email);
        Task VerifyOtpAsync(string email, string otp);
    }
}
