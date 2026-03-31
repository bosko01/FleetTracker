using FleetTracker.Application.Abstractions.Auth;
using FleetTracker.Application.Abstractions.Persistence;
using FleetTracker.Application.Features.Auth.Responses;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FleetTracker.Application.Features.Auth.Commands.LoginAdmin;

public sealed class LoginAdminCommandHandler : IRequestHandler<LoginAdminCommand, AdminLoginResponse?>
{
    private readonly IAdminUserRepository _adminUserRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly ILogger<LoginAdminCommandHandler> _logger;

    public LoginAdminCommandHandler(
        IAdminUserRepository adminUserRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenService jwtTokenService,
        ILogger<LoginAdminCommandHandler> logger)
    {
        _adminUserRepository = adminUserRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenService = jwtTokenService;
        _logger = logger;
    }

    public async Task<AdminLoginResponse?> Handle(LoginAdminCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Login attempt for user: {Username}", request.Username);

        var user = await _adminUserRepository.GetByUsernameAsync(request.Username, cancellationToken);
        if (user is null)
        {
            _logger.LogWarning("Login failed for user: {Username}. Reason: user_not_found", request.Username);
            return null;
        }

        if (!user.IsActive)
        {
            _logger.LogWarning("Login failed for user: {Username}. Reason: user_inactive", user.Username);
            return null;
        }

        var isBcryptHashFormat = user.PasswordHash.StartsWith("$2", StringComparison.Ordinal);
        var isPasswordValid = _passwordHasher.VerifyPassword(request.Password, user.PasswordHash);

        _logger.LogInformation(
            "Login credential check for user: {Username}. UserFound: true, HashLooksBcrypt: {HashLooksBcrypt}, PasswordVerified: {PasswordVerified}, Hasher: {HasherType}",
            user.Username,
            isBcryptHashFormat,
            isPasswordValid,
            _passwordHasher.GetType().Name);

        if (!isPasswordValid)
        {
            _logger.LogWarning("Login failed for user: {Username}. Reason: invalid_password", user.Username);
            return null;
        }

        var token = _jwtTokenService.GenerateToken(user.Id, user.Username, user.Role);
        _logger.LogInformation("Login successful for user: {Username}", user.Username);
        return new AdminLoginResponse(token.AccessToken, token.ExpiresAtUtc, user.Username, user.Role);
    }
}
