using System;
using System.Collections.Generic;
using System.Linq;
using Application.Dto;
using Application.Interfaces;

namespace Application.Services
{
    /// <summary>
    /// Handles the processing of customer orders by parsing input, 
    /// validating the order, and formatting the output.
    /// </summary>
    public class Server(IDishManager dishManager) : IServer
    {
        /// <summary>
        /// Processes an unparsed order string, constructs a list of dishes, 
        /// and returns the formatted result.
        /// </summary>
        /// <param name="unparsedOrder">
        /// The raw input string representing the order, 
        /// e.g., "morning, 1,2,3".
        /// </param>
        /// <returns>
        /// A formatted string of ordered dishes, such as "eggs,toast,coffee". 
        /// Returns an error message if the input is invalid.
        /// </returns>
        public string TakeOrder(string unparsedOrder)
        {
            try
            {
                // Parse the raw input string into an Order object
                var order = ParseOrder(unparsedOrder);

                // Retrieve the list of dishes based on the parsed order
                var dishes = dishManager.GetDishes(order);

                // Format and return the output string
                return FormatOutput(dishes);
            }
            catch (ApplicationException ex)
            {
                // Return a formatted error message if an exception occurs
                return $"error: {ex.Message}";
            }
        }

        /// <summary>
        /// Parses a raw input string into an <see cref="Order"/> object, 
        /// validating the period and dish IDs.
        /// </summary>
        /// <param name="unparsedOrder">
        /// The raw input string representing the order.
        /// </param>
        /// <returns>An <see cref="Order"/> object containing the parsed data.</returns>
        /// <exception cref="ApplicationException">
        /// Thrown if the input format is invalid or if the period is not supported.
        /// </exception>
        private static Order ParseOrder(string unparsedOrder)
        {
            // Split the input string into parts using commas
            var parts = unparsedOrder.Split(',');

            // Ensure the input contains at least a period and one dish ID
            if (parts.Length < 2)
            {
                throw new ApplicationException("Invalid order format. Must include period and at least one dish.");
            }

            // Extract and validate the period
            var period = parts[0].Trim().ToLower();
            if (period != "morning" && period != "evening")
            {
                throw new ApplicationException("Invalid period. Must be 'morning' or 'evening'.");
            }

            // Parse the remaining parts into a list of dish IDs
            var dishes = parts.Skip(1)
                              .Select(part =>
                              {
                                  if (int.TryParse(part.Trim(), out var dish))
                                      return dish;

                                  throw new ApplicationException("Dishes must be integers.");
                              })
                              .ToList();

            // Construct and return the Order object
            return new Order
            {
                Period = period,
                Dishes = dishes
            };
        }

        /// <summary>
        /// Formats a list of dishes into a comma-separated string, 
        /// ordered by category and including quantities for multiples.
        /// </summary>
        /// <param name="dishes">The list of dishes to format.</param>
        /// <returns>A formatted string of dishes.</returns>
        private static string FormatOutput(List<Dish> dishes)
        {
            // Define the order of categories for sorting
            var categoryOrder = new List<string> { "entrée", "side", "drink", "dessert" };

            // Sort the dishes by their category based on the defined order
            var sortedDishes = dishes
                .OrderBy(d => categoryOrder.IndexOf(d.Category!))
                .ToList();

            // Aggregate the dishes into a comma-separated string
            var returnValue = sortedDishes.Aggregate("",
                (current, dish) => current + $",{dish.DishName}{GetMultiple(dish.Count)}");

            // Remove the leading comma, if present
            if (returnValue.StartsWith(","))
            {
                returnValue = returnValue.TrimStart(',');
            }

            return returnValue;
        }

        /// <summary>
        /// Formats the count of a dish for multiples, adding "(xN)" if the count is greater than 1.
        /// </summary>
        /// <param name="count">The number of times the dish was ordered.</param>
        /// <returns>A formatted string for multiples, or an empty string for single orders.</returns>
        private static string GetMultiple(int count)
        {
            return count > 1 ? $"(x{count})" : "";
        }
    }
}
