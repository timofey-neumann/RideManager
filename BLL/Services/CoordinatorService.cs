using Domain.Interfaces;

namespace BLL.Services;

public class CoordinatorService(ICoordinatorRepository repository) : ICoordinatorService
{
    public async Task DeleteCoordinatorAsync(string email)
    {
        await repository.DeleteCoordinatorAsync(email);
    }

    public async Task AddCoordinatorAsync(string email)
    {
        await repository.AddCoordinatorAsync(email);
    }

    public async Task EditCoordinatorAsync(string email, string newEmail)
    {
        await repository.EditCoordinatorAsync(email, newEmail);
    }

    public async Task<List<string>> GetListCoordinatorsAsync()
    {
        return await repository.GetListCoordinatorsAsync();
    }
}