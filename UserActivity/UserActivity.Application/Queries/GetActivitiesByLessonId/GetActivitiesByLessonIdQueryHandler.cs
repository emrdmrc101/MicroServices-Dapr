
using Core.Domain.Mapper.Interfaces;
using Core.Tracing;
using MediatR;
using UserActivity.Application.Queries.GetActivitiesByLessonId.Objects;
using UserActivity.Domain.Entities;
using UserActivity.Domain.Interfaces;

namespace UserActivity.Application.Queries.GetActivitiesByLessonId;

public class GetActivitiesByLessonIdQueryHandler(
    IActivityRepository activityRepository,
    IMapperService mapperService,
    ActivityTracing activityTracing
) : IRequestHandler<GetActivitiesByLessonIdQuery, GetActivitiesByLessonIdQueryResponse>
{
    public async Task<GetActivitiesByLessonIdQueryResponse> Handle(GetActivitiesByLessonIdQuery request,
        CancellationToken cancellationToken)
    {
        return await activityTracing.ExecuteWithTracingAsync<GetActivitiesByLessonIdQueryResponse>(
            nameof(GetActivitiesByLessonIdQueryHandler),
            async () =>
            {
                var queryResult = await activityRepository.FindAsync(f =>
                    f.LessonId == request.LessonId
                );

                var mappedActivities = mapperService.Map<IEnumerable<Activities>, IEnumerable<Activity>>(queryResult);

                return new GetActivitiesByLessonIdQueryResponse()
                {
                    Activities = mappedActivities
                };
            });
    }
}