using FleetTracker.Application.Abstractions.Persistence;

namespace FleetTracker.Infrastructure.Persistence;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly FleetTrackerDbContext _dbContext;

    public UnitOfWork(FleetTrackerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken) => _dbContext.SaveChangesAsync(cancellationToken);
}
