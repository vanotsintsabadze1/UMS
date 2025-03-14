using UMS.Domain.Enums;

namespace UMS.Application.Models.User;

public class UserRelationshipDto
{
    public required int RelatedUserId { get; set; }
    public required UserRelationshipTypes RelationshipType { get; set; }
}