using FleetTracker.Application.Abstractions.Persistence;
using FleetTracker.Infrastructure.Persistence;
using FleetTracker.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FleetTracker.Infrastructure.DependencyInjection;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<FleetTrackerDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IVehicleRepository, VehicleRepository>();
        services.AddScoped<ITourRepository, TourRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
