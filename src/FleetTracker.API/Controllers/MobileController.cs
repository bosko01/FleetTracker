using FleetTracker.API.Contracts;
using FleetTracker.Application.Features.Mobile.Commands.CreateMobileTour;
using FleetTracker.Application.Features.Mobile.Queries.GetMobileActiveDrivers;
using FleetTracker.Application.Features.Mobile.Queries.GetMobileActiveVehicles;
using FleetTracker.Application.Features.Mobile.Queries.GetTodayToursByDriver;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FleetTracker.API.Controllers;

[ApiController]
[AllowAnonymous]
[Route("api/mobile")]
public sealed class MobileController : ControllerBase
{
    private readonly IMediator _mediator;

    public MobileController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("drivers")]
    public async Task<IActionResult> GetActiveDrivers(CancellationToken cancellationToken) =>
        Ok(await _mediator.Send(new GetMobileActiveDriversQuery(), cancellationToken));

    [HttpGet("vehicles")]
    public async Task<IActionResult> GetActiveVehicles(CancellationToken cancellationToken) =>
        Ok(await _mediator.Send(new GetMobileActiveVehiclesQuery(), cancellationToken));

    [HttpPost("tours")]
    public async Task<IActionResult> CreateTour([FromBody] CreateMobileTourRequest request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new CreateMobileTourCommand(request.DriverId, request.VehicleId, request.Date, request.UnloadCount, request.WeightKg, request.DistanceKm), cancellationToken);
        return Ok(response);
    }

    [HttpGet("drivers/{driverId:guid}/today-tours")]
    public async Task<IActionResult> GetTodayTours(Guid driverId, [FromQuery] DateOnly? date, CancellationToken cancellationToken)
    {
        var value = date ?? DateOnly.FromDateTime(DateTime.UtcNow);
        return Ok(await _mediator.Send(new GetTodayToursByDriverQuery(driverId, value), cancellationToken));
    }
}
