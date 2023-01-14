using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ZeroX.Infrastructure.Interfaces;
using ZeroX.Infrastructure.SignalR.Consumers;

namespace ZeroX.Infrastructure.Extensions;

public static class IServiceCollectionExtension
{
    public static IServiceCollection AddPublisher(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(config =>
        {
            var host = configuration["RABBITMQ_HOST"];
            if (host is null)
                throw new ArgumentNullException(nameof(host), "Host for RabbitMq is not provided");

            config.UsingRabbitMq((registrationContext, factory) =>
            {
                factory.Host(host, "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });
                factory.ReceiveEndpoint(e =>
                {
                    e.Bind("amq.fanout");
                });
                factory.ConfigureEndpoints(registrationContext);
            });
        });
        return services;
    }
    
    public static IServiceCollection AddNewRoomConsumer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(config =>
        {
            var host = configuration["RABBITMQ_HOST"];
            if (host is null)
                throw new ArgumentNullException(nameof(host), "Host for RabbitMq is not provided");

            config.AddConsumer<NewRoomConsumer>();
            config.UsingRabbitMq((registrationContext, factory) =>
            {
                factory.Host(host//"localhost"
                    , "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });
                factory.ReceiveEndpoint(e =>
                {
                    e.Bind("amq.fanout");
                    e.ConfigureConsumer<NewRoomConsumer>(registrationContext);
                });
                factory.ConfigureEndpoints(registrationContext);
            });
        });
        return services;
    }

    public static IServiceCollection AddJwt(this IServiceCollection services)
    {
        services.AddScoped<IJwtGenerator, JwtGenerator>();
        return services;
    }

    public static IServiceCollection AddCustomSignalR(this IServiceCollection services)
    {
        services.AddSignalR(options => { options.EnableDetailedErrors = true; });
        return services;
    }

    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddSwagger();
        return services;
    }
}