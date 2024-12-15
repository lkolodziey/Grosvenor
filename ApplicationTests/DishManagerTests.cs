using System;
using System.Collections.Generic;
using System.Linq;
using Application.Dto;
using Application.Interfaces;
using Application.Services;
using Domain.Interfaces;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace ApplicationTests
{
    /// <summary>
    /// Unit tests for the <see cref="DishManager"/> class, validating its logic for handling dish orders.
    /// </summary>
    [TestFixture]
    public class DishManagerTests
    {
        private ServiceProvider _serviceProvider;
        private IDishManager _dishManager;

        /// <summary>
        /// Sets up the test environment by configuring Dependency Injection (DI).
        /// </summary>
        [SetUp]
        public void Setup()
        {
            var services = new ServiceCollection();
            services.AddScoped<IDishRepository, DishRepository>();
            services.AddScoped<IDishManager, DishManager>();

            _serviceProvider = services.BuildServiceProvider();
            _dishManager = _serviceProvider.GetRequiredService<IDishManager>();
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
        /// Validates that an empty dish list returns an empty result.
        /// </summary>
        [Test]
        public void EmptyListReturnsEmptyList()
        {
            var order = new Order
            {
                Period = "morning",
                Dishes = []
            };

            var actual = _dishManager.GetDishes(order);

            Assert.That(actual.Count, Is.EqualTo(0));
        }

        /// <summary>
        /// Validates that an invalid dish ID throws an appropriate error.
        /// </summary>
        [Test]
        public void InvalidDishReturnsError()
        {
            var order = new Order
            {
                Period = "morning",
                Dishes = [5]
            };

            var ex = Assert.Throws<ApplicationException>(() => _dishManager.GetDishes(order));
            Assert.That(ex?.Message, Is.EqualTo("Invalid dish type for this period."));
        }

        /// <summary>
        /// Validates that a valid morning order returns the correct dishes.
        /// </summary>
        [Test]
        public void MorningWithValidDishesReturnsCorrectOutput()
        {
            var order = new Order
            {
                Period = "morning",
                Dishes = [1, 2, 3]
            };

            var actual = _dishManager.GetDishes(order);

            Assert.That(actual.Count, Is.EqualTo(3));
            Assert.That(actual[0].DishName, Is.EqualTo("eggs"));
            Assert.That(actual[1].DishName, Is.EqualTo("toast"));
            Assert.That(actual[2].DishName, Is.EqualTo("coffee"));
        }

        /// <summary>
        /// Validates that ordering a dessert in the morning throws an error.
        /// </summary>
        [Test]
        public void MorningWithDessertReturnsError()
        {
            var order = new Order
            {
                Period = "morning",
                Dishes = [4]
            };

            var ex = Assert.Throws<ApplicationException>(() => _dishManager.GetDishes(order));
            Assert.That(ex?.Message, Is.EqualTo("Invalid dish type for this period."));
        }

        /// <summary>
        /// Validates that multiple coffees in the morning are handled correctly.
        /// </summary>
        [Test]
        public void MorningWithMultipleCoffeesReturnsCorrectCount()
        {
            var order = new Order
            {
                Period = "morning",
                Dishes = [3, 3, 3]
            };

            var actual = _dishManager.GetDishes(order);

            Assert.That(actual.Count, Is.EqualTo(1));
            Assert.That(actual.First().DishName, Is.EqualTo("coffee"));
            Assert.That(actual.First().Count, Is.EqualTo(3));
        }

        /// <summary>
        /// Validates that a valid evening order returns the correct dishes.
        /// </summary>
        [Test]
        public void EveningWithValidDishesReturnsCorrectOutput()
        {
            var order = new Order
            {
                Period = "evening",
                Dishes = [1, 2, 3, 4]
            };

            var actual = _dishManager.GetDishes(order);

            Assert.That(actual.Count, Is.EqualTo(4));
            Assert.That(actual[0].DishName, Is.EqualTo("steak"));
            Assert.That(actual[1].DishName, Is.EqualTo("potato"));
            Assert.That(actual[2].DishName, Is.EqualTo("wine"));
            Assert.That(actual[3].DishName, Is.EqualTo("cake"));
        }

        /// <summary>
        /// Validates that multiple potatoes in the evening are handled correctly.
        /// </summary>
        [Test]
        public void EveningWithMultiplePotatoesReturnsCorrectCount()
        {
            var order = new Order
            {
                Period = "evening",
                Dishes = [2, 2]
            };

            var actual = _dishManager.GetDishes(order);

            Assert.That(actual.Count, Is.EqualTo(1));
            Assert.That(actual.First().DishName, Is.EqualTo("potato"));
            Assert.That(actual.First().Count, Is.EqualTo(2));
        }

        /// <summary>
        /// Validates that multiple steaks in the evening throw an error.
        /// </summary>
        [Test]
        public void EveningWithMultipleSteaksReturnsError()
        {
            var order = new Order
            {
                Period = "evening",
                Dishes = [1, 1]
            };

            var ex = Assert.Throws<ApplicationException>(() => _dishManager.GetDishes(order));
            Assert.That(ex?.Message, Is.EqualTo("Multiple steak(s) not allowed."));
        }

        /// <summary>
        /// Validates that an invalid dish in the evening throws an error.
        /// </summary>
        [Test]
        public void EveningWithInvalidDishReturnsError()
        {
            var order = new Order
            {
                Period = "evening",
                Dishes = [5]
            };

            var ex = Assert.Throws<ApplicationException>(() => _dishManager.GetDishes(order));
            Assert.That(ex?.Message, Is.EqualTo("Invalid dish type for this period."));
        }

        /// <summary>
        /// Validates that a valid period without dishes returns an empty list.
        /// </summary>
        [Test]
        public void ValidPeriodWithoutDishesReturnsEmptyList()
        {
            var order = new Order
            {
                Period = "morning",
                Dishes = []
            };

            var actual = _dishManager.GetDishes(order);

            Assert.That(actual.Count, Is.EqualTo(0));
        }

        /// <summary>
        /// Validates that spaces in the input do not affect the resulting dish list.
        /// </summary>
        [Test]
        public void InputWithSpacesReturnsCorrectDishList()
        {
            var order = new Order
            {
                Period = "morning",
                Dishes = [1, 2, 3]
            };

            var actual = _dishManager.GetDishes(order);
            Assert.That(actual.Count, Is.EqualTo(3));
            Assert.That(actual[0].DishName, Is.EqualTo("eggs"));
            Assert.That(actual[1].DishName, Is.EqualTo("toast"));
            Assert.That(actual[2].DishName, Is.EqualTo("coffee"));
        }
    }
}
