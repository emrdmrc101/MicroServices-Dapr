using MassTransit;
using Shared.QueueNames;

namespace UserActivity.Infrastructure.Consumers.CreateActivity;

public class CreateUserActivityConsumerDefination : ConsumerDefinition<CreateUserActivityConsumer>
{
    public CreateUserActivityConsumerDefination()
    {
        EndpointName = UserActivityQueueNames.CreateActivity;
    }

    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<CreateUserActivityConsumer> consumerConfigurator,
        IRegistrationContext context)
    {
        consumerConfigurator.UseMessageRetry(r => r.Interval(3, TimeSpan.FromMinutes(3)));
        consumerConfigurator.UseScheduledRedelivery(r =>
        {
            r.Intervals(TimeSpan.FromHours(6), TimeSpan.FromHours(12), TimeSpan.FromHours(24));
        });
        
        if (endpointConfigurator is IRabbitMqReceiveEndpointConfigurator rabbitMqEndpointConfigurator)
        {
            //
        }
        
        base.ConfigureConsumer(endpointConfigurator, consumerConfigurator, context);
    }
}