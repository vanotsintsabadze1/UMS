using UMS.Domain.Enums;

namespace UMS.Application.Models.User;

public class UserDetailedSearchRequestModel
{
    public string? Firstname { get; set; }
    public string? Lastname { get; set; } 
    public Gender? Gender { get; set; }
    public string? SocialNumber { get; set; }
    public int? CityId { get; set; }
    public ICollection<string>? PhoneNumbers { get; set; } 
}