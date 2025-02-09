using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace UserActivity.Domain.Entities;

public class BaseEntity
{
    public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.UtcNow;
    
    [BsonRepresentation(BsonType.String)]
    public Guid CreatorId { get; set; }
    
    public DateTimeOffset? LastUpdatedDate { get; set; }
    public Guid? LastUpdatedById { get; set; }
}