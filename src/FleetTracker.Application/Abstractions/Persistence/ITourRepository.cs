using FleetTracker.Domain.Entities;

namespace FleetTracker.Application.Abstractions.Persistence;

public interface ITourRepository
{
    Task AddAsync(Tour tour, CancellationToken cancellationToken);
    Task<Tour?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyList<Tour>> GetByVehicleAsync(Guid vehicleId, CancellationToken cancellationToken);
    Task<IReadOnlyList<Tour>> GetByVehicleAndDateAsync(Guid vehicleId, DateOnly date, CancellationToken cancellationToken);
    Task<bool> ExistsByVehicleDateAndTourNumberAsync(Guid vehicleId, DateOnly date, int tourNumber, Guid? excludeId, CancellationToken cancellationToken);
    Task<decimal> GetTotalDistanceByVehicleAsync(Guid vehicleId, CancellationToken cancellationToken);
    void Update(Tour tour);
    void Remove(Tour tour);
}
