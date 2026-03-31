using FleetTracker.Domain.Entities;

namespace FleetTracker.Application.Abstractions.Persistence;

public interface IVehicleRepository
{
    Task AddAsync(Vehicle vehicle, CancellationToken cancellationToken);
    Task<Vehicle?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Vehicle?> GetByRegistrationPlateAsync(string registrationPlate, CancellationToken cancellationToken);
    Task<IReadOnlyList<Vehicle>> GetAllAsync(CancellationToken cancellationToken);
    Task<bool> ExistsByRegistrationPlateAsync(string registrationPlate, Guid? excludeId, CancellationToken cancellationToken);
    void Update(Vehicle vehicle);
    void SoftDelete(Vehicle vehicle);
}
