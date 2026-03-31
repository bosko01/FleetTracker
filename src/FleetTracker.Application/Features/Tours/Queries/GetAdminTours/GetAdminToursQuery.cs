using FleetTracker.Application.Features.Tours.Responses;
using MediatR;

namespace FleetTracker.Application.Features.Tours.Queries.GetAdminTours;

public sealed record GetAdminToursQuery(DateOnly? Date, Guid? VehicleId, Guid? DriverId) : IRequest<IReadOnlyList<TourListItemResponse>>;
