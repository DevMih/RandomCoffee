using System.ComponentModel.DataAnnotations;

namespace RandomCoffee.DTOs
{
    public class RegisterDTO
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? SecondName { get; set; }
        [Required]
        public string? City { get; set; }
        [Required]
        [StringLength(8, MinimumLength = 4)]
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }
    }
}
