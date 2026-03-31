using FleetTracker.Domain.Common;
using FleetTracker.Domain.Exceptions;

namespace FleetTracker.Domain.Entities;

public sealed class Driver : BaseEntity
{
    private const int MaxFullNameLength = 200;
    private readonly List<Tour> _tours = new();

    private Driver() { }

    private Driver(Guid id, string fullName, DateTime utcNow)
    {
        Id = id;
        FullName = NormalizeFullName(fullName);
        IsActive = true;
        CreatedAtUtc = utcNow;
    }

    public string FullName { get; private set; } = string.Empty;
    public bool IsActive { get; private set; }

    public IReadOnlyCollection<Tour> Tours => _tours;

    public static Driver Create(string fullName, DateTime? utcNow = null)
    {
        ValidateFullName(fullName);
        return new Driver(Guid.NewGuid(), fullName, utcNow ?? DateTime.UtcNow);
    }

    public void UpdateDetails(string fullName, DateTime? utcNow = null)
    {
        ValidateFullName(fullName);
        FullName = NormalizeFullName(fullName);
        MarkModified(utcNow ?? DateTime.UtcNow);
    }

    public void Activate(DateTime? utcNow = null)
    {
        if (IsActive)
            return;

        IsActive = true;
        MarkModified(utcNow ?? DateTime.UtcNow);
    }

    public void Deactivate(DateTime? utcNow = null)
    {
        if (!IsActive)
            return;

        IsActive = false;
        MarkModified(utcNow ?? DateTime.UtcNow);
    }

    private static void ValidateFullName(string fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new DomainRuleViolationException("Driver full name is required.");

        if (NormalizeFullName(fullName).Length > MaxFullNameLength)
            throw new DomainRuleViolationException($"Driver full name must not exceed {MaxFullNameLength} characters.");
    }

    private static string NormalizeFullName(string fullName) => fullName.Trim();
}
