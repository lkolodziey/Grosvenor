using System;
using System.Collections.Generic;
using Application.Dto;
using Application.Interfaces;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;

namespace ApplicationTests;

/// <summary>
/// Unit tests for the <see cref="Server"/> class, validating its behavior when processing orders.
/// </summary>
[TestFixture]
public class ServerTests
{
    private ServiceProvider _serviceProvider;
    private Mock<IDishManager> _mockDishManager;

    /// <summary>
    /// Sets up the test environment by configuring Dependency Injection (DI)
    /// and mocking the <see cref="IDishManager"/> dependency.
    /// </summary>
    [SetUp]
    public void Setup()
    {
        _mockDishManager = new Mock<IDishManager>();
        var services = new ServiceCollection();
        services.AddScoped(_ => _mockDishManager.Object);
        services.AddScoped<IServer, Server>();

        _serviceProvider = services.BuildServiceProvider();
    }

    /// <summary>
    /// Cleans up the resources after each test.
    /// </summary>
    [TearDown]
    public void TearDown()
    {
        _serviceProvider?.Dispose();
    }

    /// <summary>
    /// Validates that an empty input string returns the appropriate error message.
    /// </summary>
    [Test]
    public void EmptyInputReturnsError()
    {
        const string order = "";
        const string expected = "error: Invalid order format. Must include period and at least one dish.";

        var server = _serviceProvider.GetRequiredService<IServer>();
        var actual = server.TakeOrder(order);

        Assert.That(actual, Is.EqualTo(expected));
    }

    /// <summary>
    /// Validates that an input without a period returns the appropriate error message.
    /// </summary>
    [Test]
    public void MissingPeriodReturnsError()
    {
        const string order = "1,2,3";
        const string expected = "error: Invalid period. Must be 'morning' or 'evening'.";

        var server = _serviceProvider.GetRequiredService<IServer>();
        var actual = server.TakeOrder(order);

        Assert.That(actual, Is.EqualTo(expected));
    }

    /// <summary>
    /// Validates that an invalid period returns the appropriate error message.
    /// </summary>
    [Test]
    public void InvalidPeriodReturnsError()
    {
        const string order = "afternoon, 1,2,3";
        const string expected = "error: Invalid period. Must be 'morning' or 'evening'.";

        var server = _serviceProvider.GetRequiredService<IServer>();
        var actual = server.TakeOrder(order);

        Assert.That(actual, Is.EqualTo(expected));
    }

    /// <summary>
    /// Validates that an invalid dish ID returns the appropriate error message.
    /// </summary>
    [Test]
    public void InvalidDishReturnsError()
    {
        const string order = "morning, 5";
        const string expected = "error: Invalid dish type for this period.";

        _mockDishManager
            .Setup(dm => dm.GetDishes(It.IsAny<Order>()))
            .Throws(new ApplicationException("Invalid dish type for this period."));

        var server = _serviceProvider.GetRequiredService<IServer>();
        var actual = server.TakeOrder(order);

        Assert.That(actual, Is.EqualTo(expected));
    }

    /// <summary>
    /// Validates that input with extra spaces is correctly processed.
    /// </summary>
    [Test]
    public void InputWithExtraSpacesReturnsCorrectOutput()
    {
        const string order = "  morning  ,  1  ,  2 ,  3 ";
        _mockDishManager
            .Setup(dm => dm.GetDishes(It.IsAny<Order>()))
            .Returns(new List<Dish>
            {
                new Dish { DishName = "eggs", Count = 1 },
                new Dish { DishName = "toast", Count = 1 },
                new Dish { DishName = "coffee", Count = 1 }
            });

        var server = _serviceProvider.GetRequiredService<IServer>();
        const string expected = "eggs,toast,coffee";

        var actual = server.TakeOrder(order);
        Assert.That(actual, Is.EqualTo(expected));
    }

    /// <summary>
    /// Validates that a valid morning order returns the correct output.
    /// </summary>
    [Test]
    public void MorningValidOrderReturnsCorrectOutput()
    {
        const string order = "morning, 1, 2, 3";
        _mockDishManager
            .Setup(dm => dm.GetDishes(It.IsAny<Order>()))
            .Returns(new List<Dish>
            {
                new Dish { DishName = "eggs", Count = 1 },
                new Dish { DishName = "toast", Count = 1 },
                new Dish { DishName = "coffee", Count = 1 }
            });

        var server = _serviceProvider.GetRequiredService<IServer>();
        const string expected = "eggs,toast,coffee";

        var actual = server.TakeOrder(order);
        Assert.That(actual, Is.EqualTo(expected));
    }

    /// <summary>
    /// Validates that multiple coffees in a morning order are correctly counted.
    /// </summary>
    [Test]
    public void MorningWithMultipleCoffeesReturnsCorrectCount()
    {
        const string order = "morning, 3,3,3";
        _mockDishManager
            .Setup(dm => dm.GetDishes(It.IsAny<Order>()))
            .Returns(new List<Dish> { new Dish { DishName = "coffee", Count = 3 } });

        var server = _serviceProvider.GetRequiredService<IServer>();
        const string expected = "coffee(x3)";

        var actual = server.TakeOrder(order);
        Assert.That(actual, Is.EqualTo(expected));
    }

    /// <summary>
    /// Validates that a morning order with a dessert returns an error.
    /// </summary>
    [Test]
    public void MorningWithDessertReturnsError()
    {
        const string order = "morning, 1, 2, 4";
        const string expected = "error: Invalid dish type for this period.";

        _mockDishManager
            .Setup(dm => dm.GetDishes(It.IsAny<Order>()))
            .Throws(new ApplicationException("Invalid dish type for this period."));

        var server = _serviceProvider.GetRequiredService<IServer>();
        var actual = server.TakeOrder(order);

        Assert.That(actual, Is.EqualTo(expected));
    }

    /// <summary>
    /// Validates that a valid evening order returns the correct output.
    /// </summary>
    [Test]
    public void EveningValidOrderReturnsCorrectOutput()
    {
        const string order = "evening, 1, 2, 3, 4";
        _mockDishManager
            .Setup(dm => dm.GetDishes(It.IsAny<Order>()))
            .Returns(new List<Dish>
            {
                new Dish { DishName = "steak", Count = 1 },
                new Dish { DishName = "potato", Count = 1 },
                new Dish { DishName = "wine", Count = 1 },
                new Dish { DishName = "cake", Count = 1 }
            });

        var server = _serviceProvider.GetRequiredService<IServer>();
        const string expected = "steak,potato,wine,cake";

        var actual = server.TakeOrder(order);
        Assert.That(actual, Is.EqualTo(expected));
    }

    /// <summary>
    /// Validates that multiple potatoes in an evening order are correctly counted.
    /// </summary>
    [Test]
    public void EveningWithMultiplePotatoesReturnsCorrectCount()
    {
        const string order = "evening, 2,2";
        _mockDishManager
            .Setup(dm => dm.GetDishes(It.IsAny<Order>()))
            .Returns(new List<Dish> { new Dish { DishName = "potato", Count = 2 } });

        var server = _serviceProvider.GetRequiredService<IServer>();
        const string expected = "potato(x2)";

        var actual = server.TakeOrder(order);
        Assert.That(actual, Is.EqualTo(expected));
    }

    /// <summary>
    /// Validates that multiple steaks in an evening order return an error.
    /// </summary>
    [Test]
    public void EveningWithMultipleSteaksReturnsError()
    {
        const string order = "evening, 1,1";
        const string expected = "error: Multiple steak(s) not allowed.";

        _mockDishManager
            .Setup(dm => dm.GetDishes(It.IsAny<Order>()))
            .Throws(new ApplicationException("Multiple steak(s) not allowed."));

        var server = _serviceProvider.GetRequiredService<IServer>();
        var actual = server.TakeOrder(order);

        Assert.That(actual, Is.EqualTo(expected));
    }

    /// <summary>
    /// Validates that an invalid dish in an evening order returns an error.
    /// </summary>
    [Test]
    public void EveningWithInvalidDishReturnsError()
    {
        const string order = "evening, 5";
        const string expected = "error: Invalid dish type for this period.";

        _mockDishManager
            .Setup(dm => dm.GetDishes(It.IsAny<Order>()))
            .Throws(new ApplicationException("Invalid dish type for this period."));

        var server = _serviceProvider.GetRequiredService<IServer>();
        var actual = server.TakeOrder(order);

        Assert.That(actual, Is.EqualTo(expected));
    }
}
