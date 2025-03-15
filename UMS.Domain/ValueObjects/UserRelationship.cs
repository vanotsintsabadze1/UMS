using UMS.Domain.Entities;
using UMS.Domain.Enums;

namespace UMS.Domain.ValueObjects;

public class UserRelationship
{
    public required int UserId { get; set; }
    public User User { get; set; }
    
    public required int RelatedUserId { get; set; }
    public User RelatedUser { get; set; }
    
    public required UserRelationshipTypes RelationshipType { get; set; }
}