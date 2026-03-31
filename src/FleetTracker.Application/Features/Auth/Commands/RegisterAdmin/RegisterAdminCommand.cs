using FleetTracker.Application.Features.Auth.Responses;
using MediatR;

namespace FleetTracker.Application.Features.Auth.Commands.RegisterAdmin;

public sealed record RegisterAdminCommand(string Username, string Password, string ConfirmPassword) : IRequest<RegisterAdminResponse>;
