using FleetTracker.Application.Abstractions.Persistence;
using FleetTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FleetTracker.Infrastructure.Persistence.Repositories;

public sealed class AdminUserRepository : IAdminUserRepository
{
    private readonly FleetTrackerDbContext _dbContext;

    public AdminUserRepository(FleetTrackerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<AdminUser?> GetByUsernameAsync(string username, CancellationToken cancellationToken) =>
        await _dbContext.AdminUsers.FirstOrDefaultAsync(x => x.Username == username.Trim().ToLower(), cancellationToken);

    public async Task<bool> AnyAsync(CancellationToken cancellationToken) =>
        await _dbContext.AdminUsers.AnyAsync(cancellationToken);

    public Task AddAsync(AdminUser adminUser, CancellationToken cancellationToken) => _dbContext.AdminUsers.AddAsync(adminUser, cancellationToken).AsTask();

    public void Update(AdminUser adminUser) => _dbContext.AdminUsers.Update(adminUser);
}
