using FleetTracker.Application.Features.Tours.Queries.GetAdminTours;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FleetTracker.API.Controllers;

[ApiController]
[Authorize(Roles = "Admin")]
[Route("api/admin/tours")]
public sealed class AdminToursController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdminToursController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] DateOnly? date, [FromQuery] Guid? vehicleId, [FromQuery] Guid? driverId, CancellationToken cancellationToken) =>
        Ok(await _mediator.Send(new GetAdminToursQuery(date, vehicleId, driverId), cancellationToken));
}
