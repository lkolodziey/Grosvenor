namespace Application.Interfaces
{
    /// <summary>
    /// Defines the contract for handling and processing customer orders, 
    /// including parsing input and returning formatted results.
    /// </summary>
    public interface IServer
    {
        /// <summary>
        /// Processes an unparsed order string and returns a formatted, comma-separated 
        /// list of dish names with their counts (if the count is greater than 1).
        /// </summary>
        /// <param name="unparsedOrder">
        /// A string representing the customer's order, such as "1,2,3".
        /// The string should include dish IDs separated by commas and optionally specify 
        /// the period (e.g., "morning, 1,2,3").
        /// </param>
        /// <returns>
        /// A formatted string representing the ordered dishes, such as "steak,potato,wine".
        /// If the input is invalid, the returned string starts with "error: " followed by the error message.
        /// </returns>
        string TakeOrder(string unparsedOrder);
    }
}