using System;
using Application.Interfaces;
using Application.Services;
using Domain.Interfaces;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

// Configure Dependency Injection (DI) container
var serviceProvider = new ServiceCollection()
    // Register the DishManager as the implementation of IDishManager
    .AddScoped<IDishManager, DishManager>()
    // Register the Server as the implementation of IServer
    .AddScoped<IServer, Server>()
    // Register the DishRepository as the implementation of IDishRepository
    .AddScoped<IDishRepository, DishRepository>()
    // Build the DI container to provide services
    .BuildServiceProvider();

// Resolve an instance of IServer from the DI container
var server = serviceProvider.GetRequiredService<IServer>();

// Display welcome message and instructions to the user
Console.WriteLine("Welcome to the meal ordering system!");
Console.WriteLine("Examples of valid input:");
Console.WriteLine("------------");
Console.WriteLine("Morning");
Console.WriteLine("1 - Eggs");
Console.WriteLine("2 - Toast");
Console.WriteLine("3 - Coffee");
Console.WriteLine("------------");
Console.WriteLine("Evening");
Console.WriteLine("1 - Steak");
Console.WriteLine("2 - Potato");
Console.WriteLine("3 - Wine");
Console.WriteLine("4 - Cake");
Console.WriteLine("------------");
Console.WriteLine("To make an order, follow the instructions below:");
Console.WriteLine("M[m]orning, 1,2,3 [enter] or E[e]vening, 1,2,3,4 [enter]");
Console.WriteLine("Type 'exit' to quit the system.");
Console.WriteLine("Enter your order:");

// Main loop to continuously accept user input
while (true)
{
    // Read input from the user
    var unparsedOrder = Console.ReadLine();
    
    // Check if the user wants to exit the program
    if (string.Equals(unparsedOrder, "exit", StringComparison.OrdinalIgnoreCase))
    {
        Console.WriteLine("Exiting the meal ordering system. Goodbye!");
        break; // Exit the loop
    }

    // Ensure the input is not null
    if (unparsedOrder != null)
    {
        // Process the input using the server instance
        var output = server.TakeOrder(unparsedOrder);
        Console.WriteLine(output);

        // Provide feedback for errors
        if (output.StartsWith("error:"))
        {
            Console.WriteLine("Please correct your input and try again.");
        }
    }
    else
    {
        // Handle the case where the input is null or empty
        Console.WriteLine("Please enter your input and try again.");
    }
}
