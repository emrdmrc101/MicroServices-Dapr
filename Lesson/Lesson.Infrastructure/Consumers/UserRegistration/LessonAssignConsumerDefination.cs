using MassTransit;

namespace Lesson.Infrastructure.Consumers.UserRegistration;

public class LessonAssignConsumerDefination : ConsumerDefinition<LessonAssignConsumer>
{
    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<LessonAssignConsumer> consumerConfigurator,
        IRegistrationContext context)
    {
        consumerConfigurator.UseMessageRetry(r => r.Interval(3, TimeSpan.FromMinutes(3)));
        consumerConfigurator.UseScheduledRedelivery(r =>
        {
            r.Intervals(TimeSpan.FromHours(6), TimeSpan.FromHours(12), TimeSpan.FromHours(24));
        });
        
        base.ConfigureConsumer(endpointConfigurator, consumerConfigurator, context);
    }
}