using FleetTracker.API.Contracts;
using FleetTracker.Application.Features.Drivers.Commands.ActivateDriver;
using FleetTracker.Application.Features.Drivers.Commands.CreateDriver;
using FleetTracker.Application.Features.Drivers.Commands.DeactivateDriver;
using FleetTracker.Application.Features.Drivers.Commands.UpdateDriver;
using FleetTracker.Application.Features.Drivers.Queries.GetAllDrivers;
using FleetTracker.Application.Features.Drivers.Queries.GetDriverById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FleetTracker.API.Controllers;

[ApiController]
[Authorize(Roles = "Admin")]
[Route("api/drivers")]
public sealed class DriversController : ControllerBase
{
    private readonly IMediator _mediator;

    public DriversController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateDriverRequest request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new CreateDriverCommand(request.FullName), cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken) =>
        Ok(await _mediator.Send(new GetAllDriversQuery(), cancellationToken));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken) =>
        Ok(await _mediator.Send(new GetDriverByIdQuery(id), cancellationToken));

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateDriverRequest request, CancellationToken cancellationToken) =>
        Ok(await _mediator.Send(new UpdateDriverCommand(id, request.FullName), cancellationToken));

    [HttpPatch("{id:guid}/activate")]
    public async Task<IActionResult> Activate(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new ActivateDriverCommand(id), cancellationToken);
        return NoContent();
    }

    [HttpPatch("{id:guid}/deactivate")]
    public async Task<IActionResult> Deactivate(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeactivateDriverCommand(id), cancellationToken);
        return NoContent();
    }
}
