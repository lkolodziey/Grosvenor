using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.Repositories
{
    public class DishRepository : IDishRepository
    {
        private static readonly List<DishData> Dishes =
        [
            new() { Id = 1, Name = "eggs", Category = "entrée", Period = "morning", AllowMultiple = false },
            new() { Id = 2, Name = "toast", Category = "side", Period = "morning", AllowMultiple = false },
            new() { Id = 3, Name = "coffee", Category = "drink", Period = "morning", AllowMultiple = true },

            new() { Id = 1, Name = "steak", Category = "entrée", Period = "evening", AllowMultiple = false },
            new() { Id = 2, Name = "potato", Category = "side", Period = "evening", AllowMultiple = true },
            new() { Id = 3, Name = "wine", Category = "drink", Period = "evening", AllowMultiple = false },
            new() { Id = 4, Name = "cake", Category = "dessert", Period = "evening", AllowMultiple = false }
        ];

        public DishData? GetOrderName(int order, string period)
        {
            if (!Dishes.Any(d => d.Period.Equals(period, StringComparison.CurrentCultureIgnoreCase)))
            {
                return null;
            }

            return Dishes.FirstOrDefault(d =>
                d.Id == order && d.Period.Equals(period, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}