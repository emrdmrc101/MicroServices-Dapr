using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core.ServiceBus;

public static class MassTransitConfiguration
{
    public static void AddMassTransit(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var rabbitMqAddress = configuration.GetValue<string>("MessageBroker:rabbitMq:address");
        var rabbitMqUserName = configuration.GetValue<string>("MessageBroker:rabbitMq:userName");
        var rabbitMqPassword = configuration.GetValue<string>("MessageBroker:rabbitMq:password");

        serviceCollection.AddMassTransit(x =>
        {
            x.AddConsumers(AppDomain.CurrentDomain.GetAssemblies());
            
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(rabbitMqAddress, a =>
                {
                    a.Username(rabbitMqUserName);
                    a.Password(rabbitMqPassword);
                });

                // cfg.MessageTopology.SetEntityNameFormatter(new CustomMessageNameFormatter());
                cfg.ConfigureEndpoints(context);
            });
        });
    }
}