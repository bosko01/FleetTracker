using FleetTracker.Domain.Entities;

namespace FleetTracker.Application.Abstractions.Persistence;

public interface IDriverRepository
{
    Task AddAsync(Driver driver, CancellationToken cancellationToken);
    Task<Driver?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyList<Driver>> GetAllAsync(CancellationToken cancellationToken);
    Task<IReadOnlyList<Driver>> GetActiveAsync(CancellationToken cancellationToken);
    Task<bool> ExistsByIdAsync(Guid id, CancellationToken cancellationToken);
    void Update(Driver driver);
}
