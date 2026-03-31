using FleetTracker.Domain.Common;
using FleetTracker.Domain.Exceptions;

namespace FleetTracker.Domain.Entities;

public sealed class Vehicle : SoftDeletableEntity
{
    private readonly List<Tour> _tours = new();

    private Vehicle() { }

    private Vehicle(
        Guid id,
        string registrationPlate,
        string make,
        string model,
        int year,
        decimal payloadCapacityKg,
        bool hasRamp,
        decimal initialMileageKm,
        DateTime initialMileageRecordedAtUtc,
        DateTime utcNow)
    {
        Id = id;
        RegistrationPlate = NormalizeRegistrationPlate(registrationPlate);
        Make = make.Trim();
        Model = model.Trim();
        Year = year;
        PayloadCapacityKg = payloadCapacityKg;
        HasRamp = hasRamp;
        InitialMileageKm = initialMileageKm;
        InitialMileageRecordedAtUtc = initialMileageRecordedAtUtc;
        CreatedAtUtc = utcNow;
    }

    public string RegistrationPlate { get; private set; } = string.Empty;
    public string Make { get; private set; } = string.Empty;
    public string Model { get; private set; } = string.Empty;
    public int Year { get; private set; }
    public decimal PayloadCapacityKg { get; private set; }
    public bool HasRamp { get; private set; }
    public decimal InitialMileageKm { get; private set; }
    public DateTime InitialMileageRecordedAtUtc { get; private set; }

    public IReadOnlyCollection<Tour> Tours => _tours;

    public static Vehicle Create(string registrationPlate, string make, string model, int year, decimal payloadCapacityKg, bool hasRamp, decimal initialMileageKm, DateTime? utcNow = null)
    {
        var now = utcNow ?? DateTime.UtcNow;
        Validate(registrationPlate, make, model, year, payloadCapacityKg, initialMileageKm);
        return new Vehicle(Guid.NewGuid(), registrationPlate, make, model, year, payloadCapacityKg, hasRamp, initialMileageKm, now, now);
    }

    public void UpdateDetails(string registrationPlate, string make, string model, int year, decimal payloadCapacityKg, bool hasRamp, decimal initialMileageKm, DateTime? utcNow = null)
    {
        Validate(registrationPlate, make, model, year, payloadCapacityKg, initialMileageKm);
        RegistrationPlate = NormalizeRegistrationPlate(registrationPlate);
        Make = make.Trim();
        Model = model.Trim();
        Year = year;
        PayloadCapacityKg = payloadCapacityKg;
        HasRamp = hasRamp;
        InitialMileageKm = initialMileageKm;
        MarkModified(utcNow ?? DateTime.UtcNow);
    }

    public void SoftDelete(DateTime? utcNow = null) => MarkDeleted(utcNow ?? DateTime.UtcNow);

    private static void Validate(string registrationPlate, string make, string model, int year, decimal payloadCapacityKg, decimal initialMileageKm)
    {
        if (string.IsNullOrWhiteSpace(registrationPlate)) throw new DomainRuleViolationException("Registration plate is required.");
        if (string.IsNullOrWhiteSpace(make)) throw new DomainRuleViolationException("Make is required.");
        if (string.IsNullOrWhiteSpace(model)) throw new DomainRuleViolationException("Model is required.");

        var maxYear = DateTime.UtcNow.Year + 1;
        if (year < 1950 || year > maxYear) throw new DomainRuleViolationException($"Year must be between 1950 and {maxYear}.");
        if (payloadCapacityKg < 0) throw new DomainRuleViolationException("Payload capacity must be greater than or equal to zero.");
        if (initialMileageKm < 0) throw new DomainRuleViolationException("Initial mileage must be greater than or equal to zero.");
    }

    private static string NormalizeRegistrationPlate(string registrationPlate) => registrationPlate.Trim().ToUpperInvariant();
}
