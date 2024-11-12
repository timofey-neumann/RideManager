using BLL.Services;
using DAL.Repositories;
using Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IAuthRepository, AuthRepository>();

        services.AddScoped<ICoordinatorService, CoordinatorService>();
        services.AddScoped<ICoordinatorRepository, CoordinatorRepository>();

        services.AddScoped<IPassengerReportService, PassengerReportService>();
        services.AddScoped<IPassengerReportRepository, PassengerReportRepository>();

        services.AddScoped<ICoordinatorReportService, CoordinatorReportService>();
        services.AddScoped<ICoordinatorReportRepository, CoordinatorReportRepository>();
    }
}