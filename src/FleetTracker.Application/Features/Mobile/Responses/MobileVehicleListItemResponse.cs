namespace FleetTracker.Application.Features.Mobile.Responses;

public sealed record MobileVehicleListItemResponse(Guid Id, string RegistrationPlate, string Make, string Model, bool HasRamp);
