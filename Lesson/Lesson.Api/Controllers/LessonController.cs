using Core.Tracing;
using Lesson.Application.Queries.GetLessons;
using Lesson.Application.Queries.GetLessons.Response;
using Lesson.Application.Queries.GetMyLessons;
using Lesson.Application.Queries.GetMyLessons.Responses;
using Lesson.Infrastructure.Grpc.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lesson.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class LessonController(
    IMediator mediator,
    ActivityTracing activityTracing
) : BaseController
{
    [HttpPost("GetLessons")]
    public async Task<GetLessonsQueryResponse> GetLessons()
    {
        return await activityTracing.ExecuteWithTracingAsync<GetLessonsQueryResponse>(nameof(GetMyLessons),
            async () =>
                await mediator.Send(new GetLessonsQuery()));
   
    }

    [HttpGet("GetMyLessons")]
    public async Task<GetMyLessonsQueryResponse> GetMyLessons()
    {
        return await activityTracing.ExecuteWithTracingAsync<GetMyLessonsQueryResponse>(nameof(GetMyLessons),
            async () =>
                await mediator.Send(new GetMyLessonsQuery()));
    }
}