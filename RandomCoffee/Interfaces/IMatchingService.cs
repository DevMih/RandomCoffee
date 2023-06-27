using RandomCoffee.Entities;

namespace RandomCoffee.Interfaces
{
    public interface IMatchingService
    {
        IEnumerable<AppUser> GetMatches();
    }
}
