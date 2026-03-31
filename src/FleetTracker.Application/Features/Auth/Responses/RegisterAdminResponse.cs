namespace FleetTracker.Application.Features.Auth.Responses;

public sealed record RegisterAdminResponse(Guid Id, string Username, string Role, bool IsActive, DateTime CreatedAtUtc);
