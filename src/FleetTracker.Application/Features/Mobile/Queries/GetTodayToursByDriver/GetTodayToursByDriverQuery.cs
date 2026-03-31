using FleetTracker.Application.Features.Mobile.Responses;
using MediatR;

namespace FleetTracker.Application.Features.Mobile.Queries.GetTodayToursByDriver;

public sealed record GetTodayToursByDriverQuery(Guid DriverId, DateOnly Date) : IRequest<IReadOnlyList<MobileTourListItemResponse>>;
