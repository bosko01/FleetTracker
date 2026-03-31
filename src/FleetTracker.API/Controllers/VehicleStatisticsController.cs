using FleetTracker.Application.Features.Statistics.Queries.GetVehicleDailySummary;
using FleetTracker.Application.Features.Statistics.Queries.GetVehicleTotalMileage;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FleetTracker.API.Controllers;

[ApiController]
[Authorize(Roles = "Admin")]
[Route("api/vehicles/{vehicleId:guid}")]
public sealed class VehicleStatisticsController : ControllerBase
{
    private readonly IMediator _mediator;

    public VehicleStatisticsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("daily-summary")]
    public async Task<IActionResult> GetDailySummary(Guid vehicleId, [FromQuery] DateOnly date, CancellationToken cancellationToken) =>
        Ok(await _mediator.Send(new GetVehicleDailySummaryQuery(vehicleId, date), cancellationToken));

    [HttpGet("total-mileage")]
    public async Task<IActionResult> GetTotalMileage(Guid vehicleId, CancellationToken cancellationToken) =>
        Ok(await _mediator.Send(new GetVehicleTotalMileageQuery(vehicleId), cancellationToken));
}
