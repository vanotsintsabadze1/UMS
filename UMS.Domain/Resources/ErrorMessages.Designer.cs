﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UMS.API.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class ErrorMessages {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ErrorMessages() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("UMS.Domain.Resources.ErrorMessages", typeof(ErrorMessages).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to File not found.
        /// </summary>
        internal static string File_DoesNotExist {
            get {
                return ResourceManager.GetString("File_DoesNotExist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Uploaded file is not an image.
        /// </summary>
        internal static string Image_MustBeImage {
            get {
                return ResourceManager.GetString("Image_MustBeImage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Duplicate phone numbers are not allowed.
        /// </summary>
        internal static string PhoneNumber_CantBeDuplicate {
            get {
                return ResourceManager.GetString("PhoneNumber_CantBeDuplicate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Phone number can not be empty.
        /// </summary>
        internal static string PhoneNumber_CantBeEmpty {
            get {
                return ResourceManager.GetString("PhoneNumber_CantBeEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Property must belong to either Latin or Georgian alphabet.
        /// </summary>
        internal static string Property_MustBelongToSingleAlphabet {
            get {
                return ResourceManager.GetString("Property_MustBelongToSingleAlphabet", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Property has to be a number.
        /// </summary>
        internal static string Property_MustBeNumber {
            get {
                return ResourceManager.GetString("Property_MustBeNumber", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The provided city is invalid.
        /// </summary>
        internal static string ProvidedCity_DoesNotExist {
            get {
                return ResourceManager.GetString("ProvidedCity_DoesNotExist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No such relationship exists.
        /// </summary>
        internal static string Relationship_DoesNotExist {
            get {
                return ResourceManager.GetString("Relationship_DoesNotExist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This relationship already exists.
        /// </summary>
        internal static string Relationship_ExistsAlready {
            get {
                return ResourceManager.GetString("Relationship_ExistsAlready", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Length of the social number has to be 11.
        /// </summary>
        internal static string SocialNumber_LengthMustBeEleven {
            get {
                return ResourceManager.GetString("SocialNumber_LengthMustBeEleven", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A user with this social number already exists.
        /// </summary>
        internal static string User_AlreadyExistsWithSocialNumber {
            get {
                return ResourceManager.GetString("User_AlreadyExistsWithSocialNumber", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to User does not exist.
        /// </summary>
        internal static string User_DoesNotExist {
            get {
                return ResourceManager.GetString("User_DoesNotExist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to User must be at least 18 years old.
        /// </summary>
        internal static string User_MustBeAtLeast18YearsOld {
            get {
                return ResourceManager.GetString("User_MustBeAtLeast18YearsOld", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A user with this phone number already exists.
        /// </summary>
        internal static string User_WithPhoneNumberExists {
            get {
                return ResourceManager.GetString("User_WithPhoneNumberExists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to One or more users in the relationship are invalid.
        /// </summary>
        internal static string Users_InRelationshipDoNotExist {
            get {
                return ResourceManager.GetString("Users_InRelationshipDoNotExist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The data in the request does not follow validation rules.
        /// </summary>
        internal static string Validation {
            get {
                return ResourceManager.GetString("Validation", resourceCulture);
            }
        }
    }
}
