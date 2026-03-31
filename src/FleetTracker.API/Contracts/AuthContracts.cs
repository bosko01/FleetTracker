namespace FleetTracker.API.Contracts;

public sealed record AdminLoginRequest(string Username, string Password);
public sealed record AdminRegisterRequest(string Username, string Password, string ConfirmPassword);
