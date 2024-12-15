namespace Domain.Entities;

public class DishData
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Period { get; set; } = string.Empty;
    public bool AllowMultiple { get; set; }
}