using FleetTracker.API.Contracts;
using FleetTracker.Application.Features.Tours.Commands.CreateTour;
using FleetTracker.Application.Features.Tours.Commands.DeleteTour;
using FleetTracker.Application.Features.Tours.Commands.UpdateTour;
using FleetTracker.Application.Features.Tours.Queries.GetTourById;
using FleetTracker.Application.Features.Tours.Queries.GetToursByVehicle;
using FleetTracker.Application.Features.Tours.Queries.GetToursByVehicleAndDate;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FleetTracker.API.Controllers;

[ApiController]
public sealed class ToursController : ControllerBase
{
    private readonly IMediator _mediator;

    public ToursController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("api/vehicles/{vehicleId:guid}/tours")]
    public async Task<IActionResult> Create(Guid vehicleId, [FromBody] CreateTourRequest request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new CreateTourCommand(vehicleId, request.Date, request.TourNumber, request.UnloadCount, request.WeightKg, request.DistanceKm), cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    [HttpGet("api/vehicles/{vehicleId:guid}/tours")]
    public async Task<IActionResult> GetByVehicle(Guid vehicleId, [FromQuery] DateOnly? date, CancellationToken cancellationToken)
    {
        if (date.HasValue)
        {
            var filtered = await _mediator.Send(new GetToursByVehicleAndDateQuery(vehicleId, date.Value), cancellationToken);
            return Ok(filtered);
        }

        var all = await _mediator.Send(new GetToursByVehicleQuery(vehicleId), cancellationToken);
        return Ok(all);
    }

    [HttpGet("api/tours/{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken) =>
        Ok(await _mediator.Send(new GetTourByIdQuery(id), cancellationToken));

    [HttpPut("api/tours/{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTourRequest request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new UpdateTourCommand(id, request.Date, request.TourNumber, request.UnloadCount, request.WeightKg, request.DistanceKm), cancellationToken);
        return Ok(response);
    }

    [HttpDelete("api/tours/{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteTourCommand(id), cancellationToken);
        return NoContent();
    }
}
