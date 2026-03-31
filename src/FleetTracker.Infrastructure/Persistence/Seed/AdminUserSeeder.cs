using FleetTracker.Application.Abstractions.Auth;
using FleetTracker.Domain.Entities;
using FleetTracker.Infrastructure.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace FleetTracker.Infrastructure.Persistence.Seed;

public sealed class AdminUserSeeder
{
    private readonly FleetTrackerDbContext _dbContext;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IHostEnvironment _hostEnvironment;
    private readonly AdminSeedOptions _seedOptions;

    public AdminUserSeeder(FleetTrackerDbContext dbContext, IPasswordHasher passwordHasher, IHostEnvironment hostEnvironment, IOptions<AdminSeedOptions> seedOptions)
    {
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
        _hostEnvironment = hostEnvironment;
        _seedOptions = seedOptions.Value;
    }

    public async Task SeedAsync(CancellationToken cancellationToken)
    {
        if (!_hostEnvironment.IsDevelopment())
            return;

        if (await _dbContext.AdminUsers.AnyAsync(cancellationToken))
            return;

        var admin = AdminUser.Create(_seedOptions.Username, _passwordHasher.HashPassword(_seedOptions.Password), _seedOptions.Role);
        await _dbContext.AdminUsers.AddAsync(admin, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
