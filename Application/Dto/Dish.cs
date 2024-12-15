namespace Application.Dto;

public class Dish
{
    public required string DishName { get; set; }
    public int Count { get; set; }
    public string? Category { get; set; }
}