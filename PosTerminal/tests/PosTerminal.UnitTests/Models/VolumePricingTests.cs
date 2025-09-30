using PosTerminal.Models;

namespace PosTerminal.UnitTests.Models;

public class VolumePricingTests
{
    [Fact]
    public void Constructor_WithValidParameters_ShouldCreateVolumePricing()
    {
        // Arrange & Act
        var volumePricing = new VolumePricing(3, 3.00m);

        // Assert
        volumePricing.Quantity.ShouldBe(3);
        volumePricing.Price.ShouldBe(3.00m);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(-1)]
    public void Constructor_WithInvalidQuantity_ShouldThrowArgumentException(int invalidQuantity)
    {
        // Act & Assert
        Should.Throw<ArgumentException>(() => new VolumePricing(invalidQuantity, 3.00m))
            .Message.ShouldContain("Volume pricing quantity must be at least 2");
    }

    [Fact]
    public void Constructor_WithNegativePrice_ShouldThrowArgumentException()
    {
        // Act & Assert
        Should.Throw<ArgumentException>(() => new VolumePricing(3, -1.0m))
            .Message.ShouldContain("Volume pricing price cannot be negative");
    }

    [Fact]
    public void Constructor_WithZeroPrice_ShouldCreateVolumePricing()
    {
        // Arrange & Act
        var volumePricing = new VolumePricing(3, 0m);

        // Assert
        volumePricing.Price.ShouldBe(0m);
    }
}
