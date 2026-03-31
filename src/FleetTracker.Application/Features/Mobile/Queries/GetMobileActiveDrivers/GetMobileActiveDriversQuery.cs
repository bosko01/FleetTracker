using FleetTracker.Application.Features.Mobile.Responses;
using MediatR;

namespace FleetTracker.Application.Features.Mobile.Queries.GetMobileActiveDrivers;

public sealed record GetMobileActiveDriversQuery : IRequest<IReadOnlyList<MobileDriverListItemResponse>>;
