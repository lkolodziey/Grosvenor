using System.Collections.Generic;

namespace Application.Dto
{
    /// <summary>
    /// Represents an order made by a customer, including the period of the order and the dishes requested.
    /// </summary>
    public class Order
    {
        /// <summary>
        /// The period of the order, indicating whether it is for "morning" or "evening".
        /// This property is required and must always be set.
        /// </summary>
        public required string Period { get; set; }

        /// <summary>
        /// A list of integers representing the IDs of the dishes included in the order.
        /// Defaults to an empty list if no dishes are specified.
        /// </summary>
        public List<int> Dishes { get; set; } = [];
    }
}