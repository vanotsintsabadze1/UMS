namespace UMS.Domain.Resources;

// The sole purpose of this class existing is that
// IStringLocalizer requires a name of the file in the generic
// And since the error messages are in the ErrorMessages file
// there has to be a generic class named the same way
public class ErrorMessages { }

public static class ErrorMessageNames
{
    public static readonly string Validation = "Validation";
    public static readonly string PropertyMustBelongTosingleAlphabet = "Property_MustBelongToSingleAlphabet";
    public static readonly string PropertyMustBeNumberError = "Property_MustBeNumber";
    public static readonly string SocialNumberLengthMustBeEleven = "SocialNumber_LengthMustBeEleven";
    public static readonly string PhoneNumberCanNotBeEmpty = "PhoneNumber_CantBeEmpty";
    public static readonly string PhoneNumberCantBeDuplicate = "PhoneNumber_CantBeDuplicate";
    public static readonly string ImageMustBeImage = "Image_MustBeImage";
    public static readonly string UserDoesNotExist = "User_DoesNotExist";
    public static readonly string UserAlreadyExistsWithSocialNumber = "User_AlreadyExistsWithSocialNumber";
    public static readonly string UserWithPhoneNumberExists = "User_WithPhoneNumberExists";
    public static readonly string UserMustBeAtLeast18YearsOld = "User_MustBeAtLeast18YearsOld";
    public static readonly string ProvidedCityDoesNotExist = "ProvidedCity_DoesNotExist";
    public static readonly string UsersInRelationshipDoNotExist = "Users_InRelationshipDoNotExist";
    public static readonly string RelationshipExistsAlready = "Relationship_ExistsAlready";
    public static readonly string RelationshipDoesNotExist = "Relationship_DoesNotExist";
    public static readonly string FileDoesNotExist = "File_DoesNotExist";
}