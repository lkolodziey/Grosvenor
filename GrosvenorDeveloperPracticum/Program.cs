using System;
using Application.Interfaces;
using Application.Services;
using Domain.Interfaces;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

// Configure DI container
var serviceProvider = new ServiceCollection()
    .AddScoped<IDishManager, DishManager>()
    .AddScoped<IServer, Server>()
    .AddScoped<IDishRepository, DishRepository>()
    .BuildServiceProvider();

// Resolve the Server instance
var server = serviceProvider.GetRequiredService<IServer>();

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
Console.WriteLine("Enter your order:");

while (true)
{
    var unparsedOrder = Console.ReadLine();
    
    if (string.Equals(unparsedOrder, "exit", StringComparison.OrdinalIgnoreCase))
    {
        Console.WriteLine("Exiting the meal ordering system. Goodbye!");
        break;
    }

    if (unparsedOrder != null)
    {
        var output = server.TakeOrder(unparsedOrder);
        Console.WriteLine(output);

        if (output.StartsWith("error:"))
        {
            Console.WriteLine("Please correct your input and try again.");
        }
    }
    else
    {
        Console.WriteLine("Please enter your input and try again.");
    }
}
