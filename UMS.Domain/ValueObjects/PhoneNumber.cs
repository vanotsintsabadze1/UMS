using UMS.Domain.Entities;
using UMS.Domain.Enums;

namespace UMS.Domain.ValueObjects;

public class PhoneNumber
{
    public required int UserId { get; set; }
    public required string Number { get; set; }
    public required User User { get; set; }
    public required PhoneNumberTypes Type { get; set; }
}