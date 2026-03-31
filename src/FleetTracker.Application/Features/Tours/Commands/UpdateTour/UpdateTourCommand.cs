using FleetTracker.Application.Features.Tours.Responses;
using MediatR;

namespace FleetTracker.Application.Features.Tours.Commands.UpdateTour;

public sealed record UpdateTourCommand(
    Guid Id,
    Guid VehicleId,
    Guid DriverId,
    DateOnly Date,
    int UnloadCount,
    decimal WeightKg,
    decimal DistanceKm) : IRequest<TourResponse>;
