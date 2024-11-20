using BCrypt.Net;
using Org.BouncyCastle.Crypto.Generators;
using System;
using WebApplication5.Models;
using WebApplication5.Repositories;
using WebApplication5.Servies;
using MailKit;
using NETCore.MailKit.Core;
public class AccountService : IAccountService
{
    private readonly IAccountRepository _repository;
    private readonly IEmailService _emailService;

    public AccountService(IAccountRepository repository, IEmailService emailService)
    {
        _repository = repository;
        _emailService = emailService;
    }

    public async Task RegisterAsync(RegisterDto dto)
    {
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);
        var user = new User
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Age = dto.Age,
            Address = dto.Address,
            DateOfBirth = dto.DateOfBirth,
            Ethnicity = dto.Ethnicity
        };

        var account = new Account
        {
            Email = dto.Email,
            PasswordHash = hashedPassword,
            User = user
        };

        await _repository.CreateAsync(account);
        await SendOtpAsync(dto.Email);
    }

    public async Task<string> LoginAsync(LoginDto dto)
    {
        var account = await _repository.GetByEmailAsync(dto.Email);
        if (account == null || !BCrypt.Net.BCrypt.Verify(dto.Password, account.PasswordHash))
            throw new UnauthorizedAccessException("Invalid email or password");

        if (!account.IsVerified)
            throw new UnauthorizedAccessException("Email not verified");

        return GenerateJwtToken(account);
    }

    public async Task SendOtpAsync(string email)
    {
        var otp = new Random().Next(100000, 999999).ToString();
        var account = await _repository.GetByEmailAsync(email);

        account.Otp = otp;
        account.OtpExpiry = DateTime.UtcNow.AddMinutes(10);

        await _repository.UpdateAsync(account);
        await _emailService.SendOtpAsync(email, otp);
    }

    public async Task VerifyOtpAsync(string email, string otp)
    {
        var account = await _repository.GetByOtpAsync(email, otp);
        if (account == null)
            throw new UnauthorizedAccessException("Invalid OTP");

        account.IsVerified = true;
        account.Otp = null;
        account.OtpExpiry = null;
        await _repository.UpdateAsync(account);
    }

    private string GenerateJwtToken(Account account)
    {
        // JWT Token logic here
        return "JWT-TOKEN";
    }
}