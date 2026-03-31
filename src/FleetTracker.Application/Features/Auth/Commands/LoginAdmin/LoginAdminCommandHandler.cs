using FleetTracker.Application.Abstractions.Auth;
using FleetTracker.Application.Abstractions.Persistence;
using FleetTracker.Application.Common.Exceptions;
using FleetTracker.Application.Features.Auth.Responses;
using MediatR;

namespace FleetTracker.Application.Features.Auth.Commands.LoginAdmin;

public sealed class LoginAdminCommandHandler : IRequestHandler<LoginAdminCommand, AdminLoginResponse>
{
    private readonly IAdminUserRepository _adminUserRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenService _jwtTokenService;

    public LoginAdminCommandHandler(IAdminUserRepository adminUserRepository, IPasswordHasher passwordHasher, IJwtTokenService jwtTokenService)
    {
        _adminUserRepository = adminUserRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<AdminLoginResponse> Handle(LoginAdminCommand request, CancellationToken cancellationToken)
    {
        var user = await _adminUserRepository.GetByUsernameAsync(request.Username, cancellationToken)
                   ?? throw new NotFoundException("Invalid credentials.");

        if (!user.IsActive || !_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
            throw new NotFoundException("Invalid credentials.");

        var token = _jwtTokenService.GenerateToken(user.Id, user.Username, user.Role);
        return new AdminLoginResponse(token.AccessToken, token.ExpiresAtUtc, user.Username, user.Role);
    }
}
