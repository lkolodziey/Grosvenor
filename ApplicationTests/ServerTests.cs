using System;
using System.Collections.Generic;
using Application.Dto;
using Application.Interfaces;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;

namespace ApplicationTests
{
    [TestFixture]
    public class ServerTests
    {
        private ServiceProvider _serviceProvider;
        private Mock<IDishManager> _mockDishManager;

        [SetUp]
        public void Setup()
        {
            _mockDishManager = new Mock<IDishManager>();
            var services = new ServiceCollection();
            services.AddScoped(_ => _mockDishManager.Object);
            services.AddScoped<IServer, Server>();

            _serviceProvider = services.BuildServiceProvider();
        }

        [TearDown]
        public void TearDown()
        {
            _serviceProvider?.Dispose();
        }
        
        [Test]
        public void EmptyInputReturnsError()
        {
            const string order = "";
            const string expected = "error: Invalid order format. Must include period and at least one dish.";

            var server = _serviceProvider.GetRequiredService<IServer>();
            var actual = server.TakeOrder(order);

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void MissingPeriodReturnsError()
        {
            const string order = "1,2,3";
            const string expected = "error: Invalid period. Must be 'morning' or 'evening'.";

            var server = _serviceProvider.GetRequiredService<IServer>();
            var actual = server.TakeOrder(order);

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void InvalidPeriodReturnsError()
        {
            const string order = "afternoon, 1,2,3";
            const string expected = "error: Invalid period. Must be 'morning' or 'evening'.";

            var server = _serviceProvider.GetRequiredService<IServer>();
            var actual = server.TakeOrder(order);

            Assert.That(actual, Is.EqualTo(expected));
        }

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
        
        [Test]
        public void MorningValidOrderReturnsCorrectOutput()
        {
            const string order = "morning, 1, 2, 3";
            _mockDishManager
                .Setup(dm => dm.GetDishes(It.IsAny<Order>()))
                .Returns([
                    new Dish { DishName = "eggs", Count = 1 },
                    new Dish { DishName = "toast", Count = 1 },
                    new Dish { DishName = "coffee", Count = 1 }
                ]);

            var server = _serviceProvider.GetRequiredService<IServer>();
            const string expected = "eggs,toast,coffee";

            var actual = server.TakeOrder(order);
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void MorningWithMultipleCoffeesReturnsCorrectCount()
        {
            const string order = "morning, 3,3,3";
            _mockDishManager
                .Setup(dm => dm.GetDishes(It.IsAny<Order>()))
                .Returns([new Dish { DishName = "coffee", Count = 3 }]);

            var server = _serviceProvider.GetRequiredService<IServer>();
            const string expected = "coffee(x3)";

            var actual = server.TakeOrder(order);
            Assert.That(actual, Is.EqualTo(expected));
        }

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
        
        [Test]
        public void EveningValidOrderReturnsCorrectOutput()
        {
            const string order = "evening, 1, 2, 3, 4";
            _mockDishManager
                .Setup(dm => dm.GetDishes(It.IsAny<Order>()))
                .Returns([
                    new Dish { DishName = "steak", Count = 1 },
                    new Dish { DishName = "potato", Count = 1 },
                    new Dish { DishName = "wine", Count = 1 },
                    new Dish { DishName = "cake", Count = 1 }
                ]);

            var server = _serviceProvider.GetRequiredService<IServer>();
            const string expected = "steak,potato,wine,cake";

            var actual = server.TakeOrder(order);
            Assert.That(actual, Is.EqualTo(expected));
        }

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
}
