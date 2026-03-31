using FleetTracker.Application.Abstractions.Auth;
using FleetTracker.Application.Abstractions.Persistence;
using FleetTracker.Application.Common.Exceptions;
using FleetTracker.Application.Features.Auth.Responses;
using FleetTracker.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FleetTracker.Application.Features.Auth.Commands.RegisterAdmin;

public sealed class RegisterAdminCommandHandler : IRequestHandler<RegisterAdminCommand, RegisterAdminResponse>
{
    private readonly IAdminUserRepository _adminUserRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RegisterAdminCommandHandler> _logger;

    public RegisterAdminCommandHandler(
        IAdminUserRepository adminUserRepository,
        IPasswordHasher passwordHasher,
        IUnitOfWork unitOfWork,
        ILogger<RegisterAdminCommandHandler> logger)
    {
        _adminUserRepository = adminUserRepository;
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<RegisterAdminResponse> Handle(RegisterAdminCommand request, CancellationToken cancellationToken)
    {
        var normalizedUsername = request.Username.Trim().ToLowerInvariant();
        _logger.LogInformation("Registration attempt for user: {Username}", normalizedUsername);

        if (await _adminUserRepository.ExistsByUsernameAsync(normalizedUsername, cancellationToken))
        {
            _logger.LogWarning("Registration failed for user: {Username}", normalizedUsername);
            throw new ConflictException("Username already exists.");
        }

        var adminUser = AdminUser.Create(normalizedUsername, _passwordHasher.HashPassword(request.Password), "Admin");
        await _adminUserRepository.AddAsync(adminUser, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Registration successful for user: {Username}", adminUser.Username);
        return new RegisterAdminResponse(adminUser.Id, adminUser.Username, adminUser.Role, adminUser.IsActive, adminUser.CreatedAtUtc);
    }
}
