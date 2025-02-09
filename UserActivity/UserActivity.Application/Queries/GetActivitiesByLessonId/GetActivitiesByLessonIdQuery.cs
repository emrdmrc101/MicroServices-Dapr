using MediatR;

namespace UserActivity.Application.Queries.GetActivitiesByLessonId;

public class GetActivitiesByLessonIdQuery(Guid lessonId) : IRequest<GetActivitiesByLessonIdQueryResponse>
{
    public Guid LessonId { get; } = lessonId;
}