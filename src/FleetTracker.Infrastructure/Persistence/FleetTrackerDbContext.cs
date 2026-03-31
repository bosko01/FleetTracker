using FleetTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FleetTracker.Infrastructure.Persistence;

public sealed class FleetTrackerDbContext : DbContext
{
    public FleetTrackerDbContext(DbContextOptions<FleetTrackerDbContext> options) : base(options)
    {
    }

    public DbSet<Vehicle> Vehicles => Set<Vehicle>();
    public DbSet<Tour> Tours => Set<Tour>();
    public DbSet<Driver> Drivers => Set<Driver>();
    public DbSet<AdminUser> AdminUsers => Set<AdminUser>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FleetTrackerDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
