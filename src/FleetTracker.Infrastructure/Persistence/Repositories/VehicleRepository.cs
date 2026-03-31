using FleetTracker.Application.Abstractions.Persistence;
using FleetTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FleetTracker.Infrastructure.Persistence.Repositories;

public sealed class VehicleRepository : IVehicleRepository
{
    private readonly FleetTrackerDbContext _dbContext;
    private readonly ILogger<VehicleRepository> _logger;

    public VehicleRepository(FleetTrackerDbContext dbContext, ILogger<VehicleRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public Task AddAsync(Vehicle vehicle, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Created Vehicle: {RegistrationPlate}", vehicle.RegistrationPlate);
        return _dbContext.Vehicles.AddAsync(vehicle, cancellationToken).AsTask();
    }

    public async Task<Vehicle?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        await _dbContext.Vehicles.IgnoreQueryFilters().FirstOrDefaultAsync(v => v.Id == id, cancellationToken);

    public async Task<Vehicle?> GetByRegistrationPlateAsync(string registrationPlate, CancellationToken cancellationToken) =>
        await _dbContext.Vehicles.IgnoreQueryFilters().FirstOrDefaultAsync(v => v.RegistrationPlate == registrationPlate.Trim().ToUpper(), cancellationToken);

    public async Task<IReadOnlyList<Vehicle>> GetAllAsync(CancellationToken cancellationToken) =>
        await _dbContext.Vehicles.AsNoTracking().OrderBy(v => v.RegistrationPlate).ToListAsync(cancellationToken);

    public async Task<bool> ExistsByRegistrationPlateAsync(string registrationPlate, Guid? excludeId, CancellationToken cancellationToken)
    {
        var normalized = registrationPlate.Trim().ToUpperInvariant();

        return await _dbContext.Vehicles.IgnoreQueryFilters()
            .AnyAsync(v => v.RegistrationPlate == normalized && (!excludeId.HasValue || v.Id != excludeId.Value), cancellationToken);
    }

    public void Update(Vehicle vehicle)
    {
        _logger.LogInformation("Updated Vehicle: {RegistrationPlate}", vehicle.RegistrationPlate);
        _dbContext.Vehicles.Update(vehicle);
    }

    public void SoftDelete(Vehicle vehicle)
    {
        _logger.LogInformation("Deleted Vehicle: {RegistrationPlate}", vehicle.RegistrationPlate);
        vehicle.SoftDelete();
        _dbContext.Vehicles.Update(vehicle);
    }
}
