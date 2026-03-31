using FleetTracker.Application.Abstractions.Persistence;
using FleetTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FleetTracker.Infrastructure.Persistence.Repositories;

public sealed class TourRepository : ITourRepository
{
    private readonly FleetTrackerDbContext _dbContext;

    public TourRepository(FleetTrackerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task AddAsync(Tour tour, CancellationToken cancellationToken) => _dbContext.Tours.AddAsync(tour, cancellationToken).AsTask();

    public async Task<Tour?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        await _dbContext.Tours.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

    public async Task<IReadOnlyList<Tour>> GetByVehicleAsync(Guid vehicleId, CancellationToken cancellationToken) =>
        await _dbContext.Tours.AsNoTracking()
            .Where(t => t.VehicleId == vehicleId)
            .OrderByDescending(t => t.Date)
            .ThenBy(t => t.TourNumber)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<Tour>> GetByVehicleAndDateAsync(Guid vehicleId, DateOnly date, CancellationToken cancellationToken) =>
        await _dbContext.Tours.AsNoTracking()
            .Where(t => t.VehicleId == vehicleId && t.Date == date)
            .OrderBy(t => t.TourNumber)
            .ToListAsync(cancellationToken);

    public async Task<bool> ExistsByVehicleDateAndTourNumberAsync(Guid vehicleId, DateOnly date, int tourNumber, Guid? excludeId, CancellationToken cancellationToken) =>
        await _dbContext.Tours.AnyAsync(t =>
            t.VehicleId == vehicleId &&
            t.Date == date &&
            t.TourNumber == tourNumber &&
            (!excludeId.HasValue || t.Id != excludeId.Value), cancellationToken);

    public async Task<decimal> GetTotalDistanceByVehicleAsync(Guid vehicleId, CancellationToken cancellationToken) =>
        await _dbContext.Tours.Where(t => t.VehicleId == vehicleId).SumAsync(t => t.DistanceKm, cancellationToken);

    public void Update(Tour tour) => _dbContext.Tours.Update(tour);

    public void Remove(Tour tour) => _dbContext.Tours.Remove(tour);
}
