using UserActivity.Application.Queries.GetActivitiesByLessonId.Objects;

namespace UserActivity.Application.Queries.GetActivitiesByLessonId;

public class GetActivitiesByLessonIdQueryResponse
{
    public IEnumerable<Activity> Activities { get; set; } = new List<Activity>();
}