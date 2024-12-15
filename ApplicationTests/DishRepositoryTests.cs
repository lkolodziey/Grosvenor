using Domain.Interfaces;
using Infrastructure.Repositories;
using NUnit.Framework;

namespace ApplicationTests;

[TestFixture]
public class DishRepositoryTests
{
    private IDishRepository _repository;

    [SetUp]
    public void Setup()
    {
        _repository = new DishRepository();
    }
    
    [Test]
    public void GetOrderName_ValidDishForMorning_ReturnsCorrectDish()
    {
        var result = _repository.GetOrderName(1, "morning");

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Name, Is.EqualTo("eggs"));
        Assert.That(result.Category, Is.EqualTo("entrée"));
        Assert.That(result.Period, Is.EqualTo("morning"));
    }

    [Test]
    public void GetOrderName_ValidDishForEvening_ReturnsCorrectDish()
    {
        var result = _repository.GetOrderName(3, "evening");

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Name, Is.EqualTo("wine"));
        Assert.That(result.Category, Is.EqualTo("drink"));
        Assert.That(result.Period, Is.EqualTo("evening"));
    }

    [Test]
    public void GetOrderName_PeriodCaseInsensitive_ReturnsCorrectDish()
    {
        var result = _repository.GetOrderName(1, "Morning");

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Name, Is.EqualTo("eggs"));
        Assert.That(result.Category, Is.EqualTo("entrée"));
        Assert.That(result.Period, Is.EqualTo("morning"));
    }
    
    [Test]
    public void GetOrderName_InvalidDishForMorning_ReturnsNull()
    {
        var result = _repository.GetOrderName(5, "morning");

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetOrderName_InvalidDishForEvening_ReturnsNull()
    {
        var result = _repository.GetOrderName(6, "evening");

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetOrderName_InvalidPeriod_ReturnsNull()
    {
        var result = _repository.GetOrderName(1, "afternoon");

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetOrderName_DishIdNotFound_ReturnsNull()
    {
        var result = _repository.GetOrderName(99, "morning");

        Assert.That(result, Is.Null);
    }
    
    [Test]
    public void GetOrderName_MultipleValidCalls_ReturnsCorrectResults()
    {
        var morningResult = _repository.GetOrderName(2, "morning");
        var eveningResult = _repository.GetOrderName(4, "evening");

        Assert.That(morningResult, Is.Not.Null);
        Assert.That(morningResult!.Name, Is.EqualTo("toast"));
        Assert.That(morningResult.Period, Is.EqualTo("morning"));

        Assert.That(eveningResult, Is.Not.Null);
        Assert.That(eveningResult!.Name, Is.EqualTo("cake"));
        Assert.That(eveningResult.Period, Is.EqualTo("evening"));
    }
}