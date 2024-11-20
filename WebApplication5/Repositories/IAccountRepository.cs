using WebApplication5.Models;

namespace WebApplication5.Repositories
{
    public interface IAccountRepository
    {
        Task<Account> GetByEmailAsync(string email);
        Task CreateAsync(Account account);
        Task UpdateAsync(Account account);
        Task<Account> GetByOtpAsync(string email, string otp);
    }
}