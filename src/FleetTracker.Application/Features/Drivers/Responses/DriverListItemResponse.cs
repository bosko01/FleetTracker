namespace FleetTracker.Application.Features.Drivers.Responses;

public sealed record DriverListItemResponse(Guid Id, string FullName, bool IsActive);
