using FleetTracker.Application.Features.Tours.Responses;
using MediatR;

namespace FleetTracker.Application.Features.Tours.Commands.UpdateTour;

public sealed record UpdateTourCommand(
    Guid Id,
    DateOnly Date,
    int TourNumber,
    int UnloadCount,
    decimal WeightKg,
    decimal DistanceKm) : IRequest<TourResponse>;
