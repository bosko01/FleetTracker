using FleetTracker.Application.Features.Tours.Responses;
using MediatR;

namespace FleetTracker.Application.Features.Tours.Commands.CreateTour;

public sealed record CreateTourCommand(
    Guid VehicleId,
    DateOnly Date,
    int TourNumber,
    int UnloadCount,
    decimal WeightKg,
    decimal DistanceKm) : IRequest<TourResponse>;
