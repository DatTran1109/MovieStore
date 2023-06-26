using Microsoft.AspNetCore.Identity;

namespace MovieStore.Models.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
