using FleetTracker.Application.Features.Tours.Responses;
using MediatR;

namespace FleetTracker.Application.Features.Tours.Queries.GetTourById;

public sealed record GetTourByIdQuery(Guid Id) : IRequest<TourResponse>;
