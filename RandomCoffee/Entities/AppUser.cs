using Microsoft.AspNetCore.Identity;

namespace RandomCoffee.Entities
{
    public class AppUser : IdentityUser<int>
    {
        public string? FirstName { get; set; }
        public string? SecondName { get; set; }
        public string? Introduction { get; set; }
        public string? Interests { get; set; }
        public string? City { get; set; }
        public string? Occupation { get; set; }
        public Photo Photo { get; set; }
        public ICollection<AppUserRole> UserRoles { get; set; }

        public string GetFullName()
        {
            return FirstName + " " + SecondName;
        }
    }
}
