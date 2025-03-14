using UMS.Application.Models.PhoneNumber;
using UMS.Domain.Enums;

namespace UMS.Application.Models.User;

public class UserRequestModel
{
    public required string Firstname { get; set; }
    public required string Lastname { get; set; }
    public required Gender Gender { get; set; }
    public required string SocialNumber { get; set; }
    public required DateOnly DateOfBirth { get; set; }
    public required int CityId { get; set; }
    public required ICollection<PhoneNumberDto> PhoneNumbers { get; set; }
    public ICollection<UserRelationshipDto>? Relationships { get; set; }
}