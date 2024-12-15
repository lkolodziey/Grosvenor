using Domain.Entities;

namespace Domain.Interfaces;

public interface IDishRepository
{
    DishData? GetOrderName(int order, string period);
}