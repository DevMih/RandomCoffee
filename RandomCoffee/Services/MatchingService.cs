using RandomCoffee.Data;
using RandomCoffee.Entities;

namespace RandomCoffee.Services
{
    public class MatchingService
    {
        private readonly IServiceProvider _serviceProvider;

        public MatchingService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IEnumerable<AppUser> GetMatches()
        {
            using (IServiceScope scope = _serviceProvider.CreateScope())
            {
                DataContext context = scope.ServiceProvider.GetRequiredService<DataContext>();

                var newOrder = context.Users.ToList();
                Random random = new Random();

                if (newOrder == null)
                {
                    return new List<AppUser>();
                }

                for (int i = 0; i < newOrder.Count; i++)
                {
                    var temp = newOrder[i];
                    var anotherIndex = random.Next(newOrder.Count);
                    newOrder[i] = newOrder[anotherIndex];
                    newOrder[anotherIndex] = temp;
                }

                return newOrder;
            }
        }
    }
}
