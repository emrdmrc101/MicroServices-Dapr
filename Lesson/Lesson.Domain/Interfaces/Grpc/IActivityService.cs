namespace Lesson.Domain.Interfaces.Grpc;

public interface IActivityService
{
    Task GetActivitiesByLessonId(string lessonId);
}