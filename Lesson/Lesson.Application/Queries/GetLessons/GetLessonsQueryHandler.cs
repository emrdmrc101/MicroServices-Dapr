using Core.Domain.Mapper.Interfaces;
using Core.Tracing;
using Lesson.Application.Queries.GetLessons.Response;
using Lesson.Domain.Interfaces.Grpc;
using Lesson.Domain.Interfaces.Repositories;
using MediatR;

namespace Lesson.Application.Queries.GetLessons;

public class GetLessonsQueryHandler(
    ILessonRepository lessonRepository,
    IMapperService mapperService,
    ActivityTracing activityTracing,
    IActivityService activityService
) : IRequestHandler<GetLessonsQuery, GetLessonsQueryResponse>
{
    public async Task<GetLessonsQueryResponse> Handle(GetLessonsQuery request, CancellationToken cancellationToken)
    {

        await activityService.GetActivitiesByLessonId("ae2fd582-ef44-4c0c-be89-001797e7c8e3");
        return await activityTracing.ExecuteWithTracingAsync<GetLessonsQueryResponse>(
            nameof(GetLessonsQueryHandler),
            async () =>
            {
                GetLessonsQueryResponse response = new();

                var lessonsDbResult = await lessonRepository.GetAllAsync();
                var dbResult = lessonsDbResult.ToList();
                var lessons = mapperService.Map<List<Domain.Entities.Lesson>, List<Response.Lesson>>(dbResult.ToList());

                response.Lessons = lessons;
                return response;
            });
    }
}