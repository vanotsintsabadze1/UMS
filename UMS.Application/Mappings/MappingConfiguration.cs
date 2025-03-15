using Mapster;
using UMS.Application.Models.User;
using UMS.Domain.Entities;

namespace UMS.Application.Mappings;

public static class MappingConfiguration
{
    public static void RegisterMaps()
    {
        TypeAdapterConfig<User, UserResponseModel>.NewConfig()
            .Map(dest => dest.Relationships, src => MapRelationships(src));

        TypeAdapterConfig<UserRequestModel, User>.NewConfig()
            .Map(dest => dest.RelatedUsers, src => src.Relationships);
    }

    private static List<UserRelationshipDto> MapRelationships(User user)
    {
        var relationships = new List<UserRelationshipDto>();
        
        if (user.RelatedUsers is not null)
        {
            relationships.AddRange(user.RelatedUsers.Select(r => new UserRelationshipDto()
            {
                RelatedUserId = r.RelatedUserId,
                RelationshipType = r.RelationshipType
            }));
        }
        
        
        if (user.RelatedByUsers is not null)
        {
            relationships.AddRange(user.RelatedByUsers.Select(r => new UserRelationshipDto()
            {
                RelatedUserId = r.UserId,
                RelationshipType = r.RelationshipType
            }));
        }

        return relationships;
    }
}