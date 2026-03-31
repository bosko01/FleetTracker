namespace FleetTracker.API.Contracts;

public sealed record CreateDriverRequest(string FullName);

public sealed record UpdateDriverRequest(string FullName);
