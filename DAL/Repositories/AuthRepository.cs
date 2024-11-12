using DAL.Persistence;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class AuthRepository(RideManagerContext context) : IAuthRepository
{
    public async Task<bool?> GetRoleByEmailAsync(string email)
    {
        // Проверка на координатора
        if (await context.TransportCoordinators.AnyAsync(u => u.Email == email))
        {
            return true;
        }

        /// Проверка на пассажира
        if (await context.Trips.AnyAsync(u => u.PassengerEmail == email))
        {
            return false;
        }

        return null;
    }
}
