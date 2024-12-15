using System.Collections.Generic;
using Application.Dto;

namespace Application.Interfaces
{
    /// <summary>
    /// Defines the contract for managing dish-related operations, 
    /// including constructing a list of dishes based on a customer's order.
    /// </summary>
    public interface IDishManager
    {
        /// <summary>
        /// Processes an order and constructs a list of dishes with their respective names and quantities.
        /// </summary>
        /// <param name="order">The customer's order, specifying the period and list of dish IDs.</param>
        /// <returns>A list of <see cref="Dish"/> objects, each containing the dish name, category, and count.</returns>
        List<Dish> GetDishes(Order order);
    }
}