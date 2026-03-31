namespace FleetTracker.Application.Common.Models;

public sealed record ErrorResponse(string Message, IReadOnlyCollection<string>? Errors = null);
