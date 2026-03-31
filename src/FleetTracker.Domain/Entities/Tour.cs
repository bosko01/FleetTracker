using FleetTracker.Domain.Common;
using FleetTracker.Domain.Exceptions;

namespace FleetTracker.Domain.Entities;

public sealed class Tour : BaseEntity
{
    private Tour() { }

    private Tour(Guid id, Guid vehicleId, DateOnly date, int tourNumber, int unloadCount, decimal weightKg, decimal distanceKm, DateTime utcNow)
    {
        Id = id;
        VehicleId = vehicleId;
        Date = date;
        TourNumber = tourNumber;
        UnloadCount = unloadCount;
        WeightKg = weightKg;
        DistanceKm = distanceKm;
        CreatedAtUtc = utcNow;
    }

    public Guid VehicleId { get; private set; }
    public DateOnly Date { get; private set; }
    public int TourNumber { get; private set; }
    public int UnloadCount { get; private set; }
    public decimal WeightKg { get; private set; }
    public decimal DistanceKm { get; private set; }

    public Vehicle? Vehicle { get; private set; }

    public static Tour Create(Guid vehicleId, DateOnly date, int tourNumber, int unloadCount, decimal weightKg, decimal distanceKm, DateTime? utcNow = null)
    {
        Validate(vehicleId, tourNumber, unloadCount, weightKg, distanceKm);
        return new Tour(Guid.NewGuid(), vehicleId, date, tourNumber, unloadCount, weightKg, distanceKm, utcNow ?? DateTime.UtcNow);
    }

    public void Update(DateOnly date, int tourNumber, int unloadCount, decimal weightKg, decimal distanceKm, DateTime? utcNow = null)
    {
        Validate(VehicleId, tourNumber, unloadCount, weightKg, distanceKm);
        Date = date;
        TourNumber = tourNumber;
        UnloadCount = unloadCount;
        WeightKg = weightKg;
        DistanceKm = distanceKm;
        MarkModified(utcNow ?? DateTime.UtcNow);
    }

    private static void Validate(Guid vehicleId, int tourNumber, int unloadCount, decimal weightKg, decimal distanceKm)
    {
        if (vehicleId == Guid.Empty) throw new DomainRuleViolationException("VehicleId is required.");
        if (tourNumber <= 0) throw new DomainRuleViolationException("Tour number must be greater than zero.");
        if (unloadCount < 0) throw new DomainRuleViolationException("Unload count must be greater than or equal to zero.");
        if (weightKg < 0) throw new DomainRuleViolationException("Weight must be greater than or equal to zero.");
        if (distanceKm <= 0) throw new DomainRuleViolationException("Distance must be greater than zero.");
    }
}
