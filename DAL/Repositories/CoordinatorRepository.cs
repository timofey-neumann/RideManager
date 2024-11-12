using DAL.Persistence;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class CoordinatorRepository(RideManagerContext context) : ICoordinatorRepository
{
    public async Task<List<string>> GetListCoordinatorsAsync()
    {
        return await context.TransportCoordinators
                             .AsNoTracking()
                             .Select(coordinator => coordinator.Email)
                             .ToListAsync();
    }

    public async Task AddCoordinatorAsync(string email)
    {
        await EnsureEmailIsUnique(email);

        await context.TransportCoordinators.AddAsync(new TransportCoordinator { Email = email });
        await SaveChangesAsync();
    }

    public async Task EditCoordinatorAsync(string email, string newEmail)
    {
        await EnsureEmailIsUnique(newEmail);

        var coordinator = await GetCoordinatorByEmailForUpdateAsync(email);
        coordinator.Email = newEmail;

        await SaveChangesAsync();
    }

    public async Task DeleteCoordinatorAsync(string email)
    {
        var coordinator = await GetCoordinatorByEmailForUpdateAsync(email);
        context.TransportCoordinators.Remove(coordinator);

        await SaveChangesAsync();
    }

    private async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }

    private async Task EnsureEmailIsUnique(string email)
    {
        if (await context.TransportCoordinators.AnyAsync(c => c.Email == email))
            throw new BadRequestException("Координатор с такой почтой уже существует.");
    }

    private async Task<TransportCoordinator> GetCoordinatorByEmailForUpdateAsync(string email)
    {
        var coordinator = await context.TransportCoordinators.FirstOrDefaultAsync(c => c.Email == email)
                         ?? throw new KeyNotFoundException("Координатор не найден.");
        return coordinator;
    }
}
