using MediatR;
using UserActivity.Domain.Entities.Objects;

namespace UserActivity.Application.Commands.CreateActivity;

public class CreateActivityCommand : IRequest<CreateActivityCommandResponse>
{
    public Guid Id { get; set; }
    public ActivityType ActivityType { get; set; }
    public ContentType ContentType { get; set; }
    public Guid ContentId { get; set; }
    public string ContentName { get; set; }
    public Guid LessonId { get; set; }
    public string LessonName { get; set; }
}