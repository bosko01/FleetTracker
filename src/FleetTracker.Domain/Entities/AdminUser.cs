using FleetTracker.Domain.Common;
using FleetTracker.Domain.Exceptions;

namespace FleetTracker.Domain.Entities;

public sealed class AdminUser : BaseEntity
{
    private const int MaxUsernameLength = 100;
    private const int MaxRoleLength = 50;

    private AdminUser() { }

    private AdminUser(Guid id, string username, string passwordHash, string role, DateTime utcNow)
    {
        Id = id;
        Username = NormalizeUsername(username);
        PasswordHash = passwordHash;
        Role = NormalizeRole(role);
        IsActive = true;
        CreatedAtUtc = utcNow;
    }

    public string Username { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public string Role { get; private set; } = string.Empty;
    public bool IsActive { get; private set; }

    public static AdminUser Create(string username, string passwordHash, string role, DateTime? utcNow = null)
    {
        Validate(username, passwordHash, role);
        return new AdminUser(Guid.NewGuid(), username, passwordHash, role, utcNow ?? DateTime.UtcNow);
    }

    public void UpdatePasswordHash(string passwordHash, DateTime? utcNow = null)
    {
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new DomainRuleViolationException("Password hash is required.");

        PasswordHash = passwordHash;
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

    private static void Validate(string username, string passwordHash, string role)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new DomainRuleViolationException("Username is required.");

        if (NormalizeUsername(username).Length > MaxUsernameLength)
            throw new DomainRuleViolationException($"Username must not exceed {MaxUsernameLength} characters.");

        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new DomainRuleViolationException("Password hash is required.");

        if (string.IsNullOrWhiteSpace(role))
            throw new DomainRuleViolationException("Role is required.");

        if (NormalizeRole(role).Length > MaxRoleLength)
            throw new DomainRuleViolationException($"Role must not exceed {MaxRoleLength} characters.");
    }

    private static string NormalizeUsername(string username) => username.Trim().ToLowerInvariant();
    private static string NormalizeRole(string role) => role.Trim();
}
