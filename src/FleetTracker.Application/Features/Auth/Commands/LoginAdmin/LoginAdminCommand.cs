using FleetTracker.Application.Features.Auth.Responses;
using MediatR;

namespace FleetTracker.Application.Features.Auth.Commands.LoginAdmin;

public sealed record LoginAdminCommand(string Username, string Password) : IRequest<AdminLoginResponse?>;
