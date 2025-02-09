namespace Shared.Events.UserActivityService.CreateUserActivity;

public interface ICreateUserActivityEvent
{

    public string ActivityType { get; set; }
    

    public string ContentType { get; set; }
    

    public string ContentId { get; set; }
    

    public string ContentName { get; set; }
    

    public string LessonId { get; set; }
    

    public string LessonName { get; set; }
}