
using Activity;
using Core.Domain.Mapper.Interfaces;
using Core.Mapper;

using Core.Tracing;
using MassTransit;
using MediatR;
using Shared.Events.UserActivityService.CreateUserActivity;
using Shared.Interfaces;
using UserActivity.Application.Commands.CreateActivity;
using UserActivity.Infrastructure.Grpc.Services;

namespace UserActivity.Infrastructure.Consumers.CreateActivity;

public class CreateUserActivityConsumer(
    IMediator mediator,
    IMapperService mapperService,
    ActivityTracing activityTracing,
    NotifyCreatedActivityService notifyCreatedActivityService,
    IUserClaimsService contextService) : IConsumer<ICreateUserActivityEvent>
{
    public async Task Consume(ConsumeContext<ICreateUserActivityEvent> context)
    {
        await activityTracing.ExecuteWithTracingAsync<Task>(nameof(CreateUserActivityConsumer), async () =>
        {
            var command = mapperService.Map<ICreateUserActivityEvent, CreateActivityCommand>(context.Message);
            await mediator.Send(command);

            notifyCreatedActivityService.PublishNotification(
                context.Message.LessonId.ToString(),
                new ActivityNotificationRequest()
                {
                    ActivityType = command.ActivityType.ToString(),
                    ContentType = command.ContentType.ToString(),
                    FirstName = contextService.UserContext.FirstName,
                    LastName = contextService.UserContext.LastName,
                    LessonId = command.LessonId.ToString(),
                    UserId = contextService.UserContext.UserId.ToString()
                });

            return Task.CompletedTask;
        });
    }
}