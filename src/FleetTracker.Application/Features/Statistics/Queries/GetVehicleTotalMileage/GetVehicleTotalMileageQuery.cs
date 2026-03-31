using FleetTracker.Application.Features.Statistics.Responses;
using MediatR;

namespace FleetTracker.Application.Features.Statistics.Queries.GetVehicleTotalMileage;

public sealed record GetVehicleTotalMileageQuery(Guid VehicleId) : IRequest<VehicleTotalMileageResponse>;
