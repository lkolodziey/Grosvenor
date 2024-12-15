namespace Domain.Entities;

/// <summary>
/// Represents the metadata for a dish, including its ID, name, category, 
/// serving period, and whether multiple servings are allowed.
/// </summary>
public class DishData
{
    /// <summary>
    /// The unique identifier for the dish.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The name of the dish (e.g., "eggs", "steak").
    /// Defaults to an empty string if not set.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The category of the dish (e.g., "entrée", "side", "drink", "dessert").
    /// Defaults to an empty string if not set.
    /// </summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// The serving period during which the dish is available 
    /// (e.g., "morning", "evening").
    /// Defaults to an empty string if not set.
    /// </summary>
    public string Period { get; set; } = string.Empty;

    /// <summary>
    /// Indicates whether multiple servings of the dish are allowed in an order.
    /// </summary>
    public bool AllowMultiple { get; set; }
}