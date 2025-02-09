using UserActivity.Domain.Entities.Objects;


namespace UserActivity.Application.Queries.GetActivitiesByLessonId.Objects;

public class Activity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public ContentType ContentType { get; set; }
    public ActivityType Type { get; set; }
    public string ContentName { get; set; }
    public Guid ContentId { get; set; }
    public Guid LessonId { get; set; }
    public string LessonName { get; set; }
    public Guid UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? Email { get; set; }
}