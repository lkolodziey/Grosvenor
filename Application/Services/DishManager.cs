using Application.Interfaces;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Application.Dto;

namespace Application.Services
{
    public class DishManager(IDishRepository dishRepository) : IDishManager
    {
        public List<Dish> GetDishes(Order order)
        {
            var result = new List<Dish>();

            foreach (var dishData in order.Dishes.Select(dishId => dishRepository.GetOrderName(dishId, order.Period)))
            {
                if (dishData == null)
                {
                    throw new ApplicationException("Invalid dish type for this period.");
                }
                
                var existingDish = result.FirstOrDefault(d => d.DishName == dishData.Name);

                if (existingDish == null)
                {
                    result.Add(new Dish
                    {
                        DishName = dishData.Name,
                        Category = dishData.Category,
                        Count = 1
                    });
                }
                else
                {
                    if (dishData.AllowMultiple)
                    {
                        existingDish.Count++;
                    }
                    else
                    {
                        throw new ApplicationException($"Multiple {dishData.Name}(s) not allowed.");
                    }
                }
            }

            return result;
        }
    }
}