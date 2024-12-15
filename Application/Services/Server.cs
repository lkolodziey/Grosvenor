using System;
using System.Collections.Generic;
using System.Linq;
using Application.Dto;
using Application.Interfaces;

namespace Application.Services
{
    public class Server(IDishManager dishManager) : IServer
    {
        public string TakeOrder(string unparsedOrder)
        {
            try
            {
                var order = ParseOrder(unparsedOrder);
                var dishes = dishManager.GetDishes(order);
                return FormatOutput(dishes);
            }
            catch (ApplicationException ex)
            {
                return $"error: {ex.Message}";
            }
        }

        private static Order ParseOrder(string unparsedOrder)
        {
            var parts = unparsedOrder.Split(',');
            if (parts.Length < 2)
            {
                throw new ApplicationException("Invalid order format. Must include period and at least one dish.");
            }

            var period = parts[0].Trim().ToLower();
            if (period != "morning" && period != "evening")
            {
                throw new ApplicationException("Invalid period. Must be 'morning' or 'evening'.");
            }

            var dishes = parts.Skip(1)
                              .Select(part =>
                              {
                                  if (int.TryParse(part.Trim(), out var dish))
                                      return dish;
                                  throw new ApplicationException("Dishes must be integers.");
                              })
                              .ToList();

            return new Order
            {
                Period = period,
                Dishes = dishes
            };
        }

        private static string FormatOutput(List<Dish> dishes)
        {
            var categoryOrder = new List<string> { "entrée", "side", "drink", "dessert" };
            
            var sortedDishes = dishes
                .OrderBy(d => categoryOrder.IndexOf(d.Category!))
                .ToList();
            
            var returnValue = sortedDishes.Aggregate("",
                (current, dish) => current + $",{dish.DishName}{GetMultiple(dish.Count)}");

            if (returnValue.StartsWith(","))
            {
                returnValue = returnValue.TrimStart(',');
            }

            return returnValue;
        }

        private static string GetMultiple(int count)
        {
            return count > 1 ? $"(x{count})" : "";
        }
    }
}
