namespace FleetTracker.Application.Features.Drivers.Responses;

public sealed record DriverResponse(Guid Id, string FullName, bool IsActive);
