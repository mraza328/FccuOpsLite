// Imports identity types
using Microsoft.AspNetCore.Identity;

// This places the class inside the Models/Domain namespace
namespace FccuOpsLite.Models.Domain
{
    // App's user object inherits from ASP.NET Core Identity's built in user type
    public class ApplicationUser : IdentityUser
    {
        // Adding customer property
        public string DisplayName { get; set; } = string.Empty;
    }
}
