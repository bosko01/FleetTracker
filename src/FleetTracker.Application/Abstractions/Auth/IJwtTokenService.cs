namespace FleetTracker.Application.Abstractions.Auth;

public interface IJwtTokenService
{
    (string AccessToken, DateTime ExpiresAtUtc) GenerateToken(Guid userId, string username, string role);
}
