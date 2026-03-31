using FleetTracker.Application.Abstractions.Persistence;
using FleetTracker.Application.Common.Exceptions;
using FleetTracker.Application.Features.Statistics.Responses;
using MediatR;

namespace FleetTracker.Application.Features.Statistics.Queries.GetVehicleDailySummary;

public sealed class GetVehicleDailySummaryQueryHandler : IRequestHandler<GetVehicleDailySummaryQuery, VehicleDailySummaryResponse>
{
    private readonly IVehicleRepository _vehicleRepository;
    private readonly ITourRepository _tourRepository;

    public GetVehicleDailySummaryQueryHandler(IVehicleRepository vehicleRepository, ITourRepository tourRepository)
    {
        _vehicleRepository = vehicleRepository;
        _tourRepository = tourRepository;
    }

    public async Task<VehicleDailySummaryResponse> Handle(GetVehicleDailySummaryQuery request, CancellationToken cancellationToken)
    {
        var vehicle = await _vehicleRepository.GetByIdAsync(request.VehicleId, cancellationToken)
                      ?? throw new NotFoundException("Vehicle not found.");

        if (vehicle.IsDeleted)
            throw new NotFoundException("Vehicle not found.");

        var tours = await _tourRepository.GetByVehicleAndDateAsync(request.VehicleId, request.Date, cancellationToken);

        return new VehicleDailySummaryResponse(
            vehicle.Id,
            vehicle.RegistrationPlate,
            request.Date,
            tours.Count,
            tours.Sum(t => t.UnloadCount),
            tours.Sum(t => t.WeightKg),
            tours.Sum(t => t.DistanceKm),
            tours.Select(t => t.DriverId).Distinct().Count());
    }
}
