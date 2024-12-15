using System.Collections.Generic;

namespace Application.Dto
{
    public class Order
    {
        public required string Period { get; set; }
        public List<int> Dishes { get; set; } = [];
    }
}