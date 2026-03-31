using FleetTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FleetTracker.Infrastructure.Persistence.Configurations;

public sealed class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
{
    public void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        builder.ToTable("Vehicles");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.RegistrationPlate).HasMaxLength(20).IsRequired();
        builder.Property(x => x.Make).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Model).HasMaxLength(100).IsRequired();
        builder.Property(x => x.PayloadCapacityKg).HasPrecision(18, 2);
        builder.Property(x => x.HasRamp).IsRequired();
        builder.Property(x => x.IsDeleted).IsRequired();
        builder.Property(x => x.CreatedAtUtc).IsRequired();
        builder.Property(x => x.ModifiedAtUtc);

        builder.HasIndex(x => x.RegistrationPlate).IsUnique();

        builder.HasMany(x => x.Tours)
            .WithOne(x => x.Vehicle)
            .HasForeignKey(x => x.VehicleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}
