using FleetTracker.Domain.Entities;

namespace FleetTracker.Application.Abstractions.Persistence;

public interface IAdminUserRepository
{
    Task<AdminUser?> GetByUsernameAsync(string username, CancellationToken cancellationToken);
    Task<bool> AnyAsync(CancellationToken cancellationToken);
    Task AddAsync(AdminUser adminUser, CancellationToken cancellationToken);
    void Update(AdminUser adminUser);
}
