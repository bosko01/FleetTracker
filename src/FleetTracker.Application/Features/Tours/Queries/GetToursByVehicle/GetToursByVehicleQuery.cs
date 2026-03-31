using FleetTracker.Application.Features.Tours.Responses;
using MediatR;

namespace FleetTracker.Application.Features.Tours.Queries.GetToursByVehicle;

public sealed record GetToursByVehicleQuery(Guid VehicleId) : IRequest<IReadOnlyList<TourListItemResponse>>;
