using PosTerminal.Models;

namespace PosTerminal.UnitTests.Models;

public class ProductTests
{
    [Fact]
    public void Constructor_WithValidParameters_ShouldCreateProduct()
    {
        // Arrange & Act
        var product = new Product("A", 1.25m);

        // Assert
        product.Code.ShouldBe("A");
        product.UnitPrice.ShouldBe(1.25m);
        product.VolumePricing.ShouldBeNull();
    }

    [Fact]
    public void Constructor_WithVolumePrice_ShouldCreateProductWithVolumePricing()
    {
        // Arrange
        var volumePricing = new VolumePricing(3, 3.00m);

        // Act
        var product = new Product("A", 1.25m, volumePricing);

        // Assert
        product.Code.ShouldBe("A");
        product.UnitPrice.ShouldBe(1.25m);
        product.VolumePricing.ShouldNotBeNull();
        product.VolumePricing.ShouldBe(volumePricing);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WithInvalidCode_ShouldThrowArgumentException(string? invalidCode)
    {
        // Act & Assert
        Should.Throw<ArgumentException>(() => new Product(invalidCode, 1.25m))
            .Message.ShouldContain("Product code cannot be null or empty");
    }

    [Fact]
    public void Constructor_WithNegativeUnitPrice_ShouldThrowArgumentException()
    {
        // Act & Assert
        Should.Throw<ArgumentException>(() => new Product("A", -1.0m))
            .Message.ShouldContain("Unit price cannot be negative");
    }

    [Fact]
    public void Constructor_WithZeroUnitPrice_ShouldCreateProduct()
    {
        // Arrange & Act
        var product = new Product("A", 0m);

        // Assert
        product.UnitPrice.ShouldBe(0m);
    }
}
