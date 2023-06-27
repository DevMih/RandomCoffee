namespace RandomCoffee.DTOs
{
    public class MemberDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string? FirstName { get; set; }
        public string? SecondName { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string PhotoUrl { get; set; }
        public string? Introduction { get; set; }
        public string? Interests { get; set; }
        public string? City { get; set; }
        public string? Occupation { get; set; }
    }
}
