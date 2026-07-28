using Microsoft.AspNetCore.Identity;

namespace NutriInsights.Infrastructure.Persistence;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser<Guid>
{
}
