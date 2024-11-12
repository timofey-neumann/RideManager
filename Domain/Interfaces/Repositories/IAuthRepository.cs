namespace Domain.Interfaces;

public interface IAuthRepository
{
    Task<bool?> GetRoleByEmailAsync(string email);
}