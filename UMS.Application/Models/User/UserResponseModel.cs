using UMS.Application.Models.PhoneNumber;
using UMS.Domain.Entities;
using UMS.Domain.Enums;

namespace UMS.Application.Models.User;

public class UserResponseModel
{
    public required int Id { get; set; }
    public required string Firstname { get; set; }
    public required string Lastname { get; set; }
    public required Gender Gender { get; set; }
    public required string SocialNumber { get; set; }
    public required DateOnly DateOfBirth { get; set; }
    public required City City { get; set; }
    public required string ImageUri { get; set; }
    public required ICollection<PhoneNumberDto> PhoneNumbers { get; set; }
    public required ICollection<UserRelationshipDto>? Relationships { get; set; }
}