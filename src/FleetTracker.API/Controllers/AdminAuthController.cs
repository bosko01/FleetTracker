using FleetTracker.API.Contracts;
using FleetTracker.Application.Features.Auth.Commands.LoginAdmin;
using FleetTracker.Application.Features.Auth.Commands.RegisterAdmin;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FleetTracker.API.Controllers;

[ApiController]
[Route("api/admin/auth")]
public sealed class AdminAuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdminAuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AdminLoginRequest request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new LoginAdminCommand(request.Username, request.Password), cancellationToken);
        return response is null ? Unauthorized() : Ok(response);
    }

    // MVP bootstrap choice: registration is currently anonymous and can later be protected with [Authorize(Roles = "Admin")].
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] AdminRegisterRequest request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new RegisterAdminCommand(request.Username, request.Password, request.ConfirmPassword), cancellationToken);
        return StatusCode(StatusCodes.Status201Created, response);
    }
}
