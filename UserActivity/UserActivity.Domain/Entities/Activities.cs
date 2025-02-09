using UserActivity.Domain.Entities.Objects;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Shared.Interfaces;

namespace UserActivity.Domain.Entities;
public class Activities : BaseEntity
{
    [BsonRepresentation(BsonType.String)]
    [BsonId] public Guid Id { get; set; } = Guid.NewGuid();
    public ContentType ContentType { get; set; }
    public ActivityType Type { get; set; }
    public string ContentName { get; set; }
    
    [BsonRepresentation(BsonType.String)]
    public Guid ContentId { get; set; }
    
    [BsonRepresentation(BsonType.String)]
    public Guid LessonId { get; set; }
    public string LessonName { get; set; }
    
    [BsonRepresentation(BsonType.String)]
    public Guid UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? Email { get; set; }
}