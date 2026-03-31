namespace FleetTracker.Infrastructure.Options;

public sealed class AdminSeedOptions
{
    public const string SectionName = "AdminSeed";

    public string Username { get; set; } = "admin";
    public string Password { get; set; } = "Admin123!";
    public string Role { get; set; } = "Admin";
}
