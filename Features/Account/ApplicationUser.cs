using Microsoft.AspNetCore.Identity;

namespace Telemedicine.API.Features.Account
{
    public class ApplicationUser : IdentityUser
    {
        
        public string Role { get; set; } = null!;
        public int RefId { get; set; }
    }
}
