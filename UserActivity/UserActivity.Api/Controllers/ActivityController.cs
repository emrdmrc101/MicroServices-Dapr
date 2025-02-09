using Core.Tracing;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Events.UserActivityService.CreateUserActivity;
using Shared.QueueNames;
using UserActivity.Application.Commands.CreateActivity;
using UserActivity.Application.Queries.GetActivitiesByLessonId;

namespace UserActivity.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ActivityController(
    IMediator mediator,
    ActivityTracing activityTracing,
    IBus bus
) : BaseController
{
    [HttpPost("Create")]
    public async Task<CreateActivityCommandResponse> CreateActivity([FromBody] CreateActivityCommand command)
    {
        // return await activityTracing.ExecuteWithTracingAsync<CreateActivityCommandResponse>(nameof(CreateActivity),
        //     async () =>
        //         await mediator.Send(command));
        
        return await activityTracing.ExecuteWithTracingAsync<CreateActivityCommandResponse>(nameof(CreateActivity),
            async () =>
            {
                var sendEndpoint = await bus.GetSendEndpoint(new Uri($"queue:{UserActivityQueueNames.CreateActivity}"));
                await sendEndpoint.Send<ICreateUserActivityEvent>(new
                {
                    ContentId = command.ContentId,
                    ActivityType = command.ActivityType.ToString(),
                    ContentName = command.ContentName,
                    ContentType = command.ContentType.ToString(),
                    LessonId = command.LessonId,
                    LessonName = command.LessonName
                });
        
                return new CreateActivityCommandResponse() { Success = true };
            });
    }
    
    [HttpGet("GetActivitiesByLessonId/{lessonId}")]
    public async Task<GetActivitiesByLessonIdQueryResponse> GetActivitiesByLessonId([FromRoute] Guid lessonId)
    {
        return await activityTracing.ExecuteWithTracingAsync<GetActivitiesByLessonIdQueryResponse>(nameof(GetActivitiesByLessonId),
            async () =>
                await mediator.Send(new GetActivitiesByLessonIdQuery(lessonId)));
    }
}