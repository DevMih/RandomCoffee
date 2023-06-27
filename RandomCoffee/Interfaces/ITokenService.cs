using RandomCoffee.Entities;

namespace RandomCoffee.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(AppUser user);
    }
}
