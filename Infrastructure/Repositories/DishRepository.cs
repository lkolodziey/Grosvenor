using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implements the <see cref="IDishRepository"/> interface to provide access 
    /// to predefined dish data based on dish ID and serving period.
    /// </summary>
    public class DishRepository : IDishRepository
    {
        /// <summary>
        /// A static list of predefined dishes representing the available options 
        /// for different serving periods (morning and evening).
        /// </summary>
        private static readonly List<DishData> Dishes =
        [
            new() { Id = 1, Name = "eggs", Category = "entrée", Period = "morning", AllowMultiple = false },
            new() { Id = 2, Name = "toast", Category = "side", Period = "morning", AllowMultiple = false },
            new() { Id = 3, Name = "coffee", Category = "drink", Period = "morning", AllowMultiple = true },

            // Evening dishes
            new() { Id = 1, Name = "steak", Category = "entrée", Period = "evening", AllowMultiple = false },
            new() { Id = 2, Name = "potato", Category = "side", Period = "evening", AllowMultiple = true },
            new() { Id = 3, Name = "wine", Category = "drink", Period = "evening", AllowMultiple = false },
            new() { Id = 4, Name = "cake", Category = "dessert", Period = "evening", AllowMultiple = false }
        ];

        /// <summary>
        /// Retrieves the metadata for a dish based on its ID and serving period.
        /// </summary>
        /// <param name="order">
        /// The ID of the dish to retrieve.
        /// </param>
        /// <param name="period">
        /// The serving period (e.g., "morning" or "evening").
        /// Case-insensitive.
        /// </param>
        /// <returns>
        /// A <see cref="DishData"/> object containing the metadata for the dish, 
        /// or <c>null</c> if the dish ID or period is invalid.
        /// </returns>
        public DishData? GetOrderName(int order, string period)
        {
            // Validate if the specified period exists in the predefined dishes
            if (!Dishes.Any(d => d.Period.Equals(period, StringComparison.CurrentCultureIgnoreCase)))
            {
                return null;
            }

            // Retrieve the first dish that matches the given ID and period
            return Dishes.FirstOrDefault(d =>
                d.Id == order && d.Period.Equals(period, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}
