using FleetTracker.API.Contracts;
using FleetTracker.Application.Features.Vehicles.Commands.CreateVehicle;
using FleetTracker.Application.Features.Vehicles.Commands.DeleteVehicle;
using FleetTracker.Application.Features.Vehicles.Commands.UpdateVehicle;
using FleetTracker.Application.Features.Vehicles.Queries.GetAllVehicles;
using FleetTracker.Application.Features.Vehicles.Queries.GetVehicleById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FleetTracker.API.Controllers;

[ApiController]
[Authorize(Roles = "Admin")]
[Route("api/vehicles")]
public sealed class VehiclesController : ControllerBase
{
    private readonly IMediator _mediator;

    public VehiclesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateVehicleRequest request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new CreateVehicleCommand(
            request.RegistrationPlate,
            request.Make,
            request.Model,
            request.Year,
            request.PayloadCapacityKg,
            request.HasRamp,
            request.InitialMileageKm), cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken) =>
        Ok(await _mediator.Send(new GetAllVehiclesQuery(), cancellationToken));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken) =>
        Ok(await _mediator.Send(new GetVehicleByIdQuery(id), cancellationToken));

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateVehicleRequest request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new UpdateVehicleCommand(
            id,
            request.RegistrationPlate,
            request.Make,
            request.Model,
            request.Year,
            request.PayloadCapacityKg,
            request.HasRamp), cancellationToken);

        return Ok(response);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteVehicleCommand(id), cancellationToken);
        return NoContent();
    }
}
