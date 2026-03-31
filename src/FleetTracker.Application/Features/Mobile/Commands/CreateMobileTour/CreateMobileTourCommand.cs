using FleetTracker.Application.Features.Tours.Responses;
using MediatR;

namespace FleetTracker.Application.Features.Mobile.Commands.CreateMobileTour;

public sealed record CreateMobileTourCommand(
    Guid DriverId,
    Guid VehicleId,
    DateOnly Date,
    int UnloadCount,
    decimal WeightKg,
    decimal DistanceKm) : IRequest<TourResponse>;
