using Microsoft.AspNetCore.Identity;

namespace RandomCoffee.Entities
{
    public class AppRole : IdentityRole<int>
    {
        public ICollection<AppUserRole> UserRoles { get; set; }
    }
}
