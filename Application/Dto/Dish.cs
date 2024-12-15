namespace Application.Dto;

/// <summary>
/// Represents a dish in an order, including its name, category, and the quantity ordered.
/// </summary>
public class Dish
{
    /// <summary>
    /// The name of the dish (e.g., "eggs", "steak").
    /// This property is required and must always have a value.
    /// </summary>
    public required string DishName { get; set; }

    /// <summary>
    /// The number of times this dish was ordered.
    /// Default is 0 if not explicitly set.
    /// </summary>
    public int Count { get; set; }

    /// <summary>
    /// The category of the dish (e.g., "entrée", "side", "drink", "dessert").
    /// This property can be null if the category is not defined.
    /// </summary>
    public string? Category { get; set; }
}