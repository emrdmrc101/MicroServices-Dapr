using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace UserActivity.Domain.Entities.Objects;

[JsonConverter(typeof(StringEnumConverter))]
public enum ActivityType
{
    LessonStarted,
    LessonCompleted,
    ContentWatched,
    ContentRead,
    AssignmentCompleted,
    QuizCompleted,
    MaterialDownloaded
}