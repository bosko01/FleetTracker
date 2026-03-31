using FleetTracker.Application.Abstractions.Auth;
using FleetTracker.Application.Abstractions.Persistence;
using FleetTracker.Application.Common.Exceptions;
using FleetTracker.Application.Features.Auth.Responses;
using FleetTracker.Domain.Entities;
using MediatR;

namespace FleetTracker.Application.Features.Auth.Commands.RegisterAdmin;

public sealed class RegisterAdminCommandHandler : IRequestHandler<RegisterAdminCommand, RegisterAdminResponse>
{
    private readonly IAdminUserRepository _adminUserRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterAdminCommandHandler(IAdminUserRepository adminUserRepository, IPasswordHasher passwordHasher, IUnitOfWork unitOfWork)
    {
        _adminUserRepository = adminUserRepository;
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
    }

    public async Task<RegisterAdminResponse> Handle(RegisterAdminCommand request, CancellationToken cancellationToken)
    {
        var normalizedUsername = request.Username.Trim().ToLowerInvariant();
        if (await _adminUserRepository.ExistsByUsernameAsync(normalizedUsername, cancellationToken))
            throw new ConflictException("Username already exists.");

        var adminUser = AdminUser.Create(normalizedUsername, _passwordHasher.HashPassword(request.Password), "Admin");
        await _adminUserRepository.AddAsync(adminUser, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new RegisterAdminResponse(adminUser.Id, adminUser.Username, adminUser.Role, adminUser.IsActive, adminUser.CreatedAtUtc);
    }
}
