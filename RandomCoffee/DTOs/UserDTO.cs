using RandomCoffee.Entities;

namespace RandomCoffee.DTOs
{
    public class UserDTO
    {
        public string Username { get; set; }
        public string Token { get; set; }
        public string PhotoUrl { get; set; }
        public bool EmailConfirmed { get; set; }
    }
}
