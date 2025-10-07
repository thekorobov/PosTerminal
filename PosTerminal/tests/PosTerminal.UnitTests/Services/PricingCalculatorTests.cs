using PosTerminal.Models;
using PosTerminal.Services;

namespace PosTerminal.UnitTests.Services;

public class PricingCalculatorTests
{
    private readonly PricingCalculator _calculator = new();

    #region CalculatePrice

    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 1.25)]
    [InlineData(2, 2.50)]
    [InlineData(5, 6.25)]
    public void CalculatePrice_ProductWithoutVolumePrice_ShouldReturnUnitPriceTimesQuantity(int quantity, decimal expectedPrice)
    {
        // Arrange
        var product = new Product("B", 1.25m);

        // Act
        decimal result = _calculator.CalculatePrice(product, quantity);

        // Assert
        result.ShouldBe(expectedPrice);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 1.25)] // 1 * $1.25
    [InlineData(2, 2.50)] // 2 * $1.25
    [InlineData(3, 3.00)] // 1 volume group * $3.00
    [InlineData(4, 4.25)] // 1 volume group * $3.00 + 1 * $1.25
    [InlineData(5, 5.50)] // 1 volume group * $3.00 + 2 * $1.25
    [InlineData(6, 6.00)] // 2 volume groups * $3.00
    [InlineData(7, 7.25)] // 2 volume groups * $3.00 + 1 * $1.25
    public void CalculatePrice_ProductWithVolumePrice_ShouldReturnOptimalPrice(int quantity, decimal expectedPrice)
    {
        // Arrange
        var volumePricing = new VolumePricing(3, 3.00m);
        var product = new Product("A", 1.25m, volumePricing);

        // Act
        decimal result = _calculator.CalculatePrice(product, quantity);

        // Assert
        result.ShouldBe(expectedPrice);
    }

    [Theory]
    [InlineData(1, 1.00)] // 1 * $1.00
    [InlineData(5, 5.00)] // 5 * $1.00 (no volume discount applied)
    [InlineData(6, 5.00)] // 1 volume group * $5.00
    [InlineData(7, 6.00)] // 1 volume group * $5.00 + 1 * $1.00
    [InlineData(12, 10.00)] // 2 volume groups * $5.00
    public void CalculatePrice_WithDifferentPackSize_ShouldCalculateCorrectly(int quantity, decimal expectedPrice)
    {
        // Arrange
        var volumePricing = new VolumePricing(6, 5.00m);
        var product = new Product("C", 1.00m, volumePricing);

        // Act
        decimal result = _calculator.CalculatePrice(product, quantity);

        // Assert
        result.ShouldBe(expectedPrice);
    }

    [Fact]
    public void CalculatePrice_WithNullProduct_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        Should.Throw<ArgumentNullException>(() => _calculator.CalculatePrice(null!, 1));
    }

    [Fact]
    public void CalculatePrice_WithNegativeQuantity_ShouldThrowArgumentException()
    {
        // Arrange
        var product = new Product("A", 1.25m);

        // Act & Assert
        Should.Throw<ArgumentException>(() => _calculator.CalculatePrice(product, -1))
            .Message.ShouldContain("Quantity cannot be negative");
    }

    #endregion

    #region CalculateBreakdown

    [Fact]
    public void CalculateBreakdown_ProductWithoutVolumePrice_ShouldReturnCorrectBreakdown()
    {
        // Arrange
        var product = new Product("B", 4.25m);

        // Act
        var breakdown = _calculator.CalculateBreakdown(product, 3);

        // Assert
        breakdown.TotalPrice.ShouldBe(12.75m); // 3 * $4.25
        breakdown.CardEligibleAmount.ShouldBe(12.75m); // All eligible for card discount
        breakdown.GrossAmount.ShouldBe(12.75m); // Gross same as unit price total
    }

    [Fact]
    public void CalculateBreakdown_ProductWithVolumePrice_ShouldHaveZeroCardEligible()
    {
        // Arrange
        var volumePricing = new VolumePricing(3, 3.00m);
        var product = new Product("A", 1.25m, volumePricing);

        // Act
        var breakdown = _calculator.CalculateBreakdown(product, 6);

        // Assert
        breakdown.TotalPrice.ShouldBe(6.00m); // 2 packs * $3.00
        breakdown.CardEligibleAmount.ShouldBe(0m); // No remainder, nothing eligible
        breakdown.GrossAmount.ShouldBe(7.50m); // 6 * $1.25 gross
    }

    [Fact]
    public void CalculateBreakdown_ProductWithVolumePrice_ShouldHaveCorrectBreakdown()
    {
        // Arrange
        var volumePricing = new VolumePricing(3, 3.00m);
        var product = new Product("A", 1.25m, volumePricing);

        // Act
        var breakdown = _calculator.CalculateBreakdown(product, 7);

        // Assert
        breakdown.TotalPrice.ShouldBe(7.25m); // 2 packs * $3.00 + 1 * $1.25
        breakdown.CardEligibleAmount.ShouldBe(1.25m); // Only remainder eligible
        breakdown.GrossAmount.ShouldBe(8.75m); // 7 * $1.25 gross
    }

    [Fact]
    public void CalculateBreakdown_ZeroQuantity_ShouldReturnZeroBreakdown()
    {
        // Arrange
        var product = new Product("A", 1.25m);

        // Act
        var breakdown = _calculator.CalculateBreakdown(product, 0);

        // Assert
        breakdown.TotalPrice.ShouldBe(0m);
        breakdown.CardEligibleAmount.ShouldBe(0m);
        breakdown.GrossAmount.ShouldBe(0m);
    }

    [Fact]
    public void CalculateBreakdown_WithNegativeQuantity_ShouldThrowArgumentException()
    {
        // Arrange
        var product = new Product("A", 1.25m);

        // Act & Assert
        Should.Throw<ArgumentException>(() => _calculator.CalculateBreakdown(product, -1))
            .Message.ShouldContain("Quantity cannot be negative");
    }

    [Fact]
    public void CalculateBreakdown_WithNullProduct_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        Should.Throw<ArgumentNullException>(() => _calculator.CalculateBreakdown(null!, 1));
    }

    #endregion
}
