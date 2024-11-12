namespace Domain.Interfaces;

public interface ICoordinatorService
{
    Task AddCoordinatorAsync(string email);
    Task DeleteCoordinatorAsync(string email);
    Task<List<string>> GetListCoordinatorsAsync();
    Task EditCoordinatorAsync(string email, string newEmail);
}