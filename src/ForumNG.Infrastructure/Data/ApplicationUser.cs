using Microsoft.AspNetCore.Identity;

namespace ForumNG.Infrastructure.Data;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser<Guid> { }
