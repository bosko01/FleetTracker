using FleetTracker.Application.Features.Tours.Responses;
using MediatR;

namespace FleetTracker.Application.Features.Tours.Queries.GetToursByVehicleAndDate;

public sealed record GetToursByVehicleAndDateQuery(Guid VehicleId, DateOnly Date) : IRequest<IReadOnlyList<TourListItemResponse>>;
