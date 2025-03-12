using UMS.Domain.Entities;
using UMS.Domain.Enums;

namespace UMS.Domain.ValueObjects;

public class UserRelationship
{
    public required int UserId { get; set; }
    public required User User { get; set; }
    
    public required int RelatedUserId { get; set; }
    public required User RelatedUser { get; set; }
    
    public required UserRelationshipTypes RelationshipType { get; set; }
}