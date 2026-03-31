namespace FleetTracker.Application.Features.Auth.Responses;

public sealed record AdminLoginResponse(
    string AccessToken,
    DateTime ExpiresAtUtc,
    string Username,
    string Role);
