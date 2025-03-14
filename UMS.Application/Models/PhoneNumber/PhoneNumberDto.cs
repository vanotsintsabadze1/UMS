using UMS.Domain.Enums;

namespace UMS.Application.Models.PhoneNumber;

public class PhoneNumberDto
{
    public required string Number { get; set; }
    public required PhoneNumberTypes Type { get; set; }
}