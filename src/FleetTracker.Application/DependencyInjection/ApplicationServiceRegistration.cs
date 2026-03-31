using FleetTracker.Application.Behaviors;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FleetTracker.Application.DependencyInjection;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ApplicationServiceRegistration).Assembly));
        services.AddValidatorsFromAssembly(typeof(ApplicationServiceRegistration).Assembly);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));

        return services;
    }
}
