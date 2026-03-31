using FleetTracker.Application.Abstractions.Persistence;
using FleetTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FleetTracker.Infrastructure.Persistence.Repositories;

public sealed class DriverRepository : IDriverRepository
{
    private readonly FleetTrackerDbContext _dbContext;
    private readonly ILogger<DriverRepository> _logger;

    public DriverRepository(FleetTrackerDbContext dbContext, ILogger<DriverRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public Task AddAsync(Driver driver, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Created Driver: {FullName}", driver.FullName);
        return _dbContext.Drivers.AddAsync(driver, cancellationToken).AsTask();
    }

    public async Task<Driver?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        await _dbContext.Drivers.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<IReadOnlyList<Driver>> GetAllAsync(CancellationToken cancellationToken) =>
        await _dbContext.Drivers.AsNoTracking().OrderBy(x => x.FullName).ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<Driver>> GetActiveAsync(CancellationToken cancellationToken) =>
        await _dbContext.Drivers.AsNoTracking().Where(x => x.IsActive).OrderBy(x => x.FullName).ToListAsync(cancellationToken);

    public async Task<bool> ExistsByIdAsync(Guid id, CancellationToken cancellationToken) =>
        await _dbContext.Drivers.AnyAsync(x => x.Id == id, cancellationToken);

    public void Update(Driver driver)
    {
        _logger.LogInformation("Updated Driver: {FullName}", driver.FullName);
        _dbContext.Drivers.Update(driver);
    }
}
