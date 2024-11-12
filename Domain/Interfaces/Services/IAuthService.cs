namespace Domain.Interfaces;

public interface IAuthService
{
    Task<(string token, string role)?> GenerateTokenByEmailAsync(string email);
}