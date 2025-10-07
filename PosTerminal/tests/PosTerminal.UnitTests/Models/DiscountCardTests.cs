using PosTerminal.Models;

namespace PosTerminal.UnitTests.Models;

public class DiscountCardTests
{
    [Fact]
    public void Constructor_WithoutInitialAmount_ShouldCreateCardWithZeroBalance()
    {
        // Arrange & Act
        var card = new DiscountCard();

        // Assert
        card.AccumulatedAmount.ShouldBe(0m);
    }

    [Fact]
    public void Constructor_WithValidInitialAmount_ShouldCreateCardWithSpecifiedBalance()
    {
        // Arrange & Act
        var card = new DiscountCard(1500m);

        // Assert
        card.AccumulatedAmount.ShouldBe(1500m);
    }

    [Fact]
    public void Constructor_WithNegativeInitialAmount_ShouldThrowArgumentException()
    {
        // Act & Assert
        Should.Throw<ArgumentException>(() => new DiscountCard(-100m))
            .Message.ShouldContain("Initial amount cannot be negative");
    }

    [Theory]
    [InlineData(0, 0.00)]
    [InlineData(500, 0.00)]
    [InlineData(999.99, 0.00)]
    public void GetPercent_BelowBronzeThreshold_ShouldReturnZeroPercent(decimal amount, decimal expectedPercent)
    {
        // Arrange
        var card = new DiscountCard(amount);

        // Act
        decimal percent = card.GetPercent();

        // Assert
        percent.ShouldBe(expectedPercent);
    }

    [Theory]
    [InlineData(1000, 0.01)]
    [InlineData(1500, 0.01)]
    [InlineData(1999.99, 0.01)]
    public void GetPercent_BronzeLevel_ShouldReturnOnePercent(decimal amount, decimal expectedPercent)
    {
        // Arrange
        var card = new DiscountCard(amount);

        // Act
        decimal percent = card.GetPercent();

        // Assert
        percent.ShouldBe(expectedPercent);
    }

    [Theory]
    [InlineData(2000, 0.03)]
    [InlineData(3000, 0.03)]
    [InlineData(4999.99, 0.03)]
    public void GetPercent_SilverLevel_ShouldReturnThreePercent(decimal amount, decimal expectedPercent)
    {
        // Arrange
        var card = new DiscountCard(amount);

        // Act
        decimal percent = card.GetPercent();

        // Assert
        percent.ShouldBe(expectedPercent);
    }

    [Theory]
    [InlineData(5000, 0.05)]
    [InlineData(7500, 0.05)]
    [InlineData(9999.99, 0.05)]
    public void GetPercent_GoldLevel_ShouldReturnFivePercent(decimal amount, decimal expectedPercent)
    {
        // Arrange
        var card = new DiscountCard(amount);

        // Act
        decimal percent = card.GetPercent();

        // Assert
        percent.ShouldBe(expectedPercent);
    }

    [Theory]
    [InlineData(10_000, 0.07)]
    [InlineData(15_000, 0.07)]
    [InlineData(50_000, 0.07)]
    public void GetPercent_PlatinumLevel_ShouldReturnSevenPercent(decimal amount, decimal expectedPercent)
    {
        // Arrange
        var card = new DiscountCard(amount);

        // Act
        decimal percent = card.GetPercent();

        // Assert
        percent.ShouldBe(expectedPercent);
    }

    [Fact]
    public void Accumulate_WithValidAmount_ShouldIncreaseAccumulatedAmount()
    {
        // Arrange
        var card = new DiscountCard(1000m);

        // Act
        card.Accumulate(500m);

        // Assert
        card.AccumulatedAmount.ShouldBe(1500m);
    }

    [Fact]
    public void Accumulate_WithZeroAmount_ShouldNotChangeAccumulatedAmount()
    {
        // Arrange
        var card = new DiscountCard(1000m);

        // Act
        card.Accumulate(0m);

        // Assert
        card.AccumulatedAmount.ShouldBe(1000m);
    }

    [Fact]
    public void Accumulate_WithNegativeAmount_ShouldThrowArgumentException()
    {
        // Arrange
        var card = new DiscountCard(1000m);

        // Act & Assert
        Should.Throw<ArgumentException>(() => card.Accumulate(-100m))
            .Message.ShouldContain("Gross sale cannot be negative");
    }

    [Fact]
    public void Accumulate_MultipleTransactions_ShouldAccumulateCorrectly()
    {
        // Arrange
        var card = new DiscountCard();

        // Act
        card.Accumulate(500m);
        card.Accumulate(700m);
        card.Accumulate(300m);

        // Assert
        card.AccumulatedAmount.ShouldBe(1500m);
    }
}

