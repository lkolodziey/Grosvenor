using Application.Interfaces;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Application.Dto;

namespace Application.Services
{
    /// <summary>
    /// Implements the logic for processing an order and constructing a list of dishes, 
    /// using the provided repository to retrieve dish data.
    /// </summary>
    public class DishManager(IDishRepository dishRepository) : IDishManager
    {
        /// <summary>
        /// Processes an order and returns a list of dishes, each with its name, category, 
        /// and count based on the provided dish IDs and the period (e.g., morning or evening).
        /// </summary>
        /// <param name="order">The customer's order, containing the period and list of dish IDs.</param>
        /// <returns>
        /// A list of <see cref="Dish"/> objects representing the ordered dishes, 
        /// including their names, categories, and quantities.
        /// </returns>
        /// <exception cref="ApplicationException">
        /// Thrown when:
        /// - A dish ID is invalid for the specified period.
        /// - A dish that does not allow multiple servings is ordered more than once.
        /// </exception>
        public List<Dish> GetDishes(Order order)
        {
            // Initialize the result list to store processed dishes
            var result = new List<Dish>();

            // Iterate over each dish ID in the order and retrieve its corresponding data
            foreach (var dishData in order.Dishes.Select(dishId => dishRepository.GetOrderName(dishId, order.Period)))
            {
                // If no dish data is found for the given ID and period, throw an exception
                if (dishData == null)
                {
                    throw new ApplicationException("Invalid dish type for this period.");
                }
                
                // Check if the dish already exists in the result list
                var existingDish = result.FirstOrDefault(d => d.DishName == dishData.Name);

                if (existingDish == null)
                {
                    // Add a new dish to the result if it is not already present
                    result.Add(new Dish
                    {
                        DishName = dishData.Name,
                        Category = dishData.Category,
                        Count = 1
                    });
                }
                else
                {
                    // If the dish allows multiple servings, increment the count
                    if (dishData.AllowMultiple)
                    {
                        existingDish.Count++;
                    }
                    else
                    {
                        // If multiple servings are not allowed, throw an exception
                        throw new ApplicationException($"Multiple {dishData.Name}(s) not allowed.");
                    }
                }
            }

            // Return the constructed list of dishes
            return result;
        }
    }
}
