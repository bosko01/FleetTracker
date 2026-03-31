using FleetTracker.Application.Abstractions.Persistence;
using FleetTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FleetTracker.Infrastructure.Persistence.Repositories;

public sealed class TourRepository : ITourRepository
{
    private readonly FleetTrackerDbContext _dbContext;
    private readonly ILogger<TourRepository> _logger;

    public TourRepository(FleetTrackerDbContext dbContext, ILogger<TourRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public Task AddAsync(Tour tour, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Created Tour: VehicleId={VehicleId}, DriverId={DriverId}, Date={Date}, TourNumber={TourNumber}",
            tour.VehicleId, tour.DriverId, tour.Date, tour.TourNumber);
        return _dbContext.Tours.AddAsync(tour, cancellationToken).AsTask();
    }

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

    public async Task<IReadOnlyList<Tour>> GetByDriverAndDateAsync(Guid driverId, DateOnly date, CancellationToken cancellationToken) =>
        await _dbContext.Tours.AsNoTracking()
            .Where(t => t.DriverId == driverId && t.Date == date)
            .OrderByDescending(t => t.CreatedAtUtc)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<Tour>> GetFilteredAsync(DateOnly? date, Guid? vehicleId, Guid? driverId, CancellationToken cancellationToken)
    {
        var query = _dbContext.Tours.AsNoTracking().AsQueryable();

        if (date.HasValue)
            query = query.Where(x => x.Date == date.Value);

        if (vehicleId.HasValue)
            query = query.Where(x => x.VehicleId == vehicleId.Value);

        if (driverId.HasValue)
            query = query.Where(x => x.DriverId == driverId.Value);

        return await query.OrderByDescending(x => x.Date).ThenBy(x => x.TourNumber).ToListAsync(cancellationToken);
    }

    public async Task<int> GetMaxTourNumberForVehicleAndDateAsync(Guid vehicleId, DateOnly date, CancellationToken cancellationToken)
    {
        var max = await _dbContext.Tours
            .Where(t => t.VehicleId == vehicleId && t.Date == date)
            .Select(t => (int?)t.TourNumber)
            .MaxAsync(cancellationToken);

        return max ?? 0;
    }

    public async Task<bool> ExistsByVehicleDateAndTourNumberAsync(Guid vehicleId, DateOnly date, int tourNumber, Guid? excludeId, CancellationToken cancellationToken) =>
        await _dbContext.Tours.AnyAsync(t =>
            t.VehicleId == vehicleId &&
            t.Date == date &&
            t.TourNumber == tourNumber &&
            (!excludeId.HasValue || t.Id != excludeId.Value), cancellationToken);

    public async Task<decimal> GetTotalDistanceByVehicleAsync(Guid vehicleId, CancellationToken cancellationToken) =>
        await _dbContext.Tours.Where(t => t.VehicleId == vehicleId).SumAsync(t => t.DistanceKm, cancellationToken);

    public void Update(Tour tour)
    {
        _logger.LogInformation("Updated Tour: Id={TourId}", tour.Id);
        _dbContext.Tours.Update(tour);
    }

    public void Remove(Tour tour)
    {
        _logger.LogInformation("Deleted Tour: Id={TourId}", tour.Id);
        _dbContext.Tours.Remove(tour);
    }
}
