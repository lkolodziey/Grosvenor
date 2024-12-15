using Domain.Interfaces;
using Infrastructure.Repositories;
using NUnit.Framework;

namespace ApplicationTests;

/// <summary>
/// Unit tests for the <see cref="DishRepository"/> class, validating its behavior
/// when retrieving dish metadata based on dish IDs and periods.
/// </summary>
[TestFixture]
public class DishRepositoryTests
{
    private IDishRepository _repository;

    /// <summary>
    /// Initializes the repository instance before each test.
    /// </summary>
    [SetUp]
    public void Setup()
    {
        _repository = new DishRepository();
    }

    /// <summary>
    /// Validates that a valid dish ID for the "morning" period returns the correct dish metadata.
    /// </summary>
    [Test]
    public void GetOrderName_ValidDishForMorning_ReturnsCorrectDish()
    {
        var result = _repository.GetOrderName(1, "morning");

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Name, Is.EqualTo("eggs"));
        Assert.That(result.Category, Is.EqualTo("entrée"));
        Assert.That(result.Period, Is.EqualTo("morning"));
    }

    /// <summary>
    /// Validates that a valid dish ID for the "evening" period returns the correct dish metadata.
    /// </summary>
    [Test]
    public void GetOrderName_ValidDishForEvening_ReturnsCorrectDish()
    {
        var result = _repository.GetOrderName(3, "evening");

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Name, Is.EqualTo("wine"));
        Assert.That(result.Category, Is.EqualTo("drink"));
        Assert.That(result.Period, Is.EqualTo("evening"));
    }

    /// <summary>
    /// Ensures that the repository correctly handles case-insensitive period values.
    /// </summary>
    [Test]
    public void GetOrderName_PeriodCaseInsensitive_ReturnsCorrectDish()
    {
        var result = _repository.GetOrderName(1, "Morning");

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Name, Is.EqualTo("eggs"));
        Assert.That(result.Category, Is.EqualTo("entrée"));
        Assert.That(result.Period, Is.EqualTo("morning"));
    }

    /// <summary>
    /// Validates that an invalid dish ID for the "morning" period returns null.
    /// </summary>
    [Test]
    public void GetOrderName_InvalidDishForMorning_ReturnsNull()
    {
        var result = _repository.GetOrderName(5, "morning");

        Assert.That(result, Is.Null);
    }

    /// <summary>
    /// Validates that an invalid dish ID for the "evening" period returns null.
    /// </summary>
    [Test]
    public void GetOrderName_InvalidDishForEvening_ReturnsNull()
    {
        var result = _repository.GetOrderName(6, "evening");

        Assert.That(result, Is.Null);
    }

    /// <summary>
    /// Validates that an invalid period returns null for a valid dish ID.
    /// </summary>
    [Test]
    public void GetOrderName_InvalidPeriod_ReturnsNull()
    {
        var result = _repository.GetOrderName(1, "afternoon");

        Assert.That(result, Is.Null);
    }

    /// <summary>
    /// Ensures that a dish ID not found in the repository returns null.
    /// </summary>
    [Test]
    public void GetOrderName_DishIdNotFound_ReturnsNull()
    {
        var result = _repository.GetOrderName(99, "morning");

        Assert.That(result, Is.Null);
    }

    /// <summary>
    /// Validates that multiple valid calls to the repository return the correct results.
    /// </summary>
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
