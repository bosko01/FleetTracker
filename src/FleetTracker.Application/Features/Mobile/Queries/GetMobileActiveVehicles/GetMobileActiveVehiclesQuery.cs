using FleetTracker.Application.Features.Mobile.Responses;
using MediatR;

namespace FleetTracker.Application.Features.Mobile.Queries.GetMobileActiveVehicles;

public sealed record GetMobileActiveVehiclesQuery : IRequest<IReadOnlyList<MobileVehicleListItemResponse>>;
