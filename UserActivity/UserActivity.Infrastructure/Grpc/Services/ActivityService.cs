using Activity;
using Core.Tracing;
using Grpc.Core;
using MediatR;
using UserActivity.Application.Queries.GetActivitiesByLessonId;

namespace UserActivity.Infrastructure.Grpc.Services;

public class ActivityService(
    IMediator mediator,
    ActivityTracing activityTracing) : ActivityServiceGrpc.ActivityServiceGrpcBase
{
    public override Task<GetActivitiesByLessonIdResponse> GetActivitiesByLessonId(
        GetActivitiesByLessonIdRequest request, ServerCallContext context)
    {
        return activityTracing.ExecuteWithTracing<Task<GetActivitiesByLessonIdResponse>>(nameof(GetActivitiesByLessonId), () =>
        {
            Guid.TryParse(request.LessonId, out Guid outLessonId);
            var queryResult = mediator.Send(new GetActivitiesByLessonIdQuery(outLessonId)).Result;

            var response = new GetActivitiesByLessonIdResponse();

            var activityItems = queryResult.Activities.Select(s => new ActivityResponse()
            {
                LessonId = s.LessonId.ToString(),
                ActivityType = s.Type.ToString(),
                ContentType = s.ContentType.ToString(),
                FirstName = s.FirstName,
                Id = s.Id.ToString(),
                LastName = s.FirstName
            });

            response.Activities.AddRange(activityItems);
            return Task.FromResult(response);
        });
    }
    
 
    

    
    
    
}