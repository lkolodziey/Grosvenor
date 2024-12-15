using Domain.Entities;

namespace Domain.Interfaces;

/// <summary>
/// Defines the contract for accessing dish-related data, 
/// such as retrieving dish metadata based on its ID and serving period.
/// </summary>
public interface IDishRepository
{
    /// <summary>
    /// Retrieves the metadata for a dish, including its name, category, 
    /// and other properties, based on the dish ID and serving period.
    /// </summary>
    /// <param name="order">
    /// The ID of the dish to retrieve (e.g., 1 for "eggs").
    /// </param>
    /// <param name="period">
    /// The serving period for the dish (e.g., "morning" or "evening").
    /// Case-insensitive.
    /// </param>
    /// <returns>
    /// A <see cref="DishData"/> object containing the dish metadata 
    /// if the dish ID and period are valid; otherwise, <c>null</c>.
    /// </returns>
    DishData? GetOrderName(int order, string period);
}