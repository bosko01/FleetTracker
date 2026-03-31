using FleetTracker.Application.Features.Statistics.Responses;
using MediatR;

namespace FleetTracker.Application.Features.Statistics.Queries.GetVehicleDailySummary;

public sealed record GetVehicleDailySummaryQuery(Guid VehicleId, DateOnly Date) : IRequest<VehicleDailySummaryResponse>;
