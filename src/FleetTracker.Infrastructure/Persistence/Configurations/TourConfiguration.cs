using FleetTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FleetTracker.Infrastructure.Persistence.Configurations;

public sealed class TourConfiguration : IEntityTypeConfiguration<Tour>
{
    public void Configure(EntityTypeBuilder<Tour> builder)
    {
        builder.ToTable("Tours");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.VehicleId).IsRequired();
        builder.Property(x => x.Date).IsRequired();
        builder.Property(x => x.TourNumber).IsRequired();
        builder.Property(x => x.UnloadCount).IsRequired();
        builder.Property(x => x.WeightKg).HasPrecision(18, 2).IsRequired();
        builder.Property(x => x.DistanceKm).HasPrecision(18, 2).IsRequired();
        builder.Property(x => x.CreatedAtUtc).IsRequired();
        builder.Property(x => x.ModifiedAtUtc);

        builder.HasIndex(x => new { x.VehicleId, x.Date, x.TourNumber }).IsUnique();
    }
}
