using PosTerminal.Models;

namespace PosTerminal.UnitTests;

public class PointOfSaleTerminalTests
{
    private PointOfSaleTerminal CreateTerminalWithStandardPricing()
    {
        var terminal = new PointOfSaleTerminal();
        var products = new[]
        {
            new Product("A", 1.25m, new VolumePricing(3, 3.00m)), // $1.25 or $3 for a three-pack
            new Product("B", 4.25m),                              // $4.25 per unit
            new Product("C", 1.00m, new VolumePricing(6, 5.00m)), // $1.00 or $5 for a six-pack
            new Product("D", 0.75m)                               // $0.75 per unit
        };
        terminal.SetPricing(products);
        return terminal;
    }

    [Fact]
    public void SetPricing_WithValidProducts_ShouldConfigureTerminal()
    {
        // Arrange
        var terminal = new PointOfSaleTerminal();
        var products = new[]
        {
            new Product("A", 1.25m),
            new Product("B", 4.25m)
        };

        // Act & Assert
        terminal.SetPricing(products);
    }

    [Fact]
    public void SetPricing_WithNullProducts_ShouldThrowArgumentNullException()
    {
        // Arrange
        var terminal = new PointOfSaleTerminal();

        // Act & Assert
        Should.Throw<ArgumentNullException>(() => terminal.SetPricing(null!));
    }

    [Fact]
    public void SetPricing_WithEmptyProducts_ShouldThrowArgumentException()
    {
        // Arrange
        var terminal = new PointOfSaleTerminal();

        // Act & Assert
        Should.Throw<ArgumentException>(() => terminal.SetPricing([]))
            .Message.ShouldContain("Products collection cannot be empty");
    }

    [Fact]
    public void SetPricing_WithDuplicateProductCodes_ShouldThrowArgumentException()
    {
        // Arrange
        var terminal = new PointOfSaleTerminal();
        var products = new[]
        {
            new Product("A", 1.25m),
            new Product("A", 2.00m)
        };

        // Act & Assert
        Should.Throw<ArgumentException>(() => terminal.SetPricing(products))
            .Message.ShouldContain("Duplicate product code found: A");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Scan_WithInvalidProductCode_ShouldThrowArgumentException(string? invalidCode)
    {
        // Arrange
        var terminal = CreateTerminalWithStandardPricing();

        // Act & Assert
        Should.Throw<ArgumentException>(() => terminal.Scan(invalidCode!))
            .Message.ShouldContain("Product code cannot be null or empty");
    }

    [Fact]
    public void Scan_WithUnknownProductCode_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var terminal = CreateTerminalWithStandardPricing();

        // Act & Assert
        Should.Throw<InvalidOperationException>(() => terminal.Scan("X"))
            .Message.ShouldContain("Product code 'X' not found");
    }

    [Fact]
    public void CalculateTotal_WithoutPricingSet_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var terminal = new PointOfSaleTerminal();

        // Act & Assert
        Should.Throw<InvalidOperationException>(() => terminal.CalculateTotal())
            .Message.ShouldContain("No pricing configuration set");
    }

    [Fact]
    public void CalculateTotal_WithNoItemsScanned_ShouldReturnZero()
    {
        // Arrange
        var terminal = CreateTerminalWithStandardPricing();

        // Act
        decimal result = terminal.CalculateTotal();

        // Assert
        result.ShouldBe(0m);
    }

    [Fact]
    public void Clear_ShouldRemoveAllScannedItems()
    {
        // Arrange
        var terminal = CreateTerminalWithStandardPricing();
        terminal.Scan("A");
        terminal.Scan("B");

        // Act
        terminal.Clear();
        decimal result = terminal.CalculateTotal();

        // Assert
        result.ShouldBe(0m);
    }

    // Test Case 1: AAAABCDAAA should total $13.25
    [Fact]
    public void CalculateTotal_TestCase1_AAAABCDAAA_ShouldReturn1325()
    {
        // Arrange
        var terminal = CreateTerminalWithStandardPricing();
        const string items = "AAAABCDAAA";

        // Act
        foreach (char item in items)
        {
            terminal.Scan(item.ToString());
        }
        decimal result = terminal.CalculateTotal();

        // Assert
        // A: 7 items = 6 for $6.00 (2 volume groups) + 1 for $1.25 = $7.25
        // B: 1 item = $4.25
        // C: 1 item = $1.00
        // D: 1 item = $0.75
        // Total: $7.25 + $4.25 + $1.00 + $0.75 = $13.25
        result.ShouldBe(13.25m);
    }

    // Test Case 2: CCCCCCC should total $6.00
    [Fact]
    public void CalculateTotal_TestCase2_CCCCCCC_ShouldReturn600()
    {
        // Arrange
        var terminal = CreateTerminalWithStandardPricing();
        const string items = "CCCCCCC";

        // Act
        foreach (char item in items)
        {
            terminal.Scan(item.ToString());
        }
        decimal result = terminal.CalculateTotal();

        // Assert
        // C: 7 items = 6 for $5.00 (1 volume group) + 1 for $1.00 = $6.00
        result.ShouldBe(6.00m);
    }

    // Test Case 3: ABCD should total $7.25
    [Fact]
    public void CalculateTotal_TestCase3_ABCD_ShouldReturn725()
    {
        // Arrange
        var terminal = CreateTerminalWithStandardPricing();
        const string items = "ABCD";

        // Act
        foreach (char item in items)
        {
            terminal.Scan(item.ToString());
        }
        decimal result = terminal.CalculateTotal();

        // Assert
        // A: 1 item = $1.25
        // B: 1 item = $4.25
        // C: 1 item = $1.00
        // D: 1 item = $0.75
        // Total: $1.25 + $4.25 + $1.00 + $0.75 = $7.25
        result.ShouldBe(7.25m);
    }

    [Fact]
    public void Scan_MultipleItemsSameProduct_ShouldAccumulateQuantity()
    {
        // Arrange
        var terminal = CreateTerminalWithStandardPricing();

        // Act
        terminal.Scan("A");
        terminal.Scan("A");
        terminal.Scan("A");
        decimal result = terminal.CalculateTotal();

        // Assert
        // 3 A items should use volume pricing: $3.00
        result.ShouldBe(3.00m);
    }

    [Fact]
    public void CalculateTotal_MixedItemsWithVolumeDiscounts_ShouldCalculateCorrectly()
    {
        // Arrange
        var terminal = CreateTerminalWithStandardPricing();

        // Act
        for (int i = 0; i < 4; i++) terminal.Scan("A");
        for (int i = 0; i < 7; i++) terminal.Scan("C");
        decimal result = terminal.CalculateTotal();

        // Assert
        // A: 4 items = 3 for $3.00 + 1 for $1.25 = $4.25
        // C: 7 items = 6 for $5.00 + 1 for $1.00 = $6.00
        // Total: $4.25 + $6.00 = $10.25
        result.ShouldBe(10.25m);
    }

    [Fact]
    public void SetPricing_WhenActiveTransactionExists_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var terminal = CreateTerminalWithStandardPricing();
        terminal.Scan("A");

        var newProducts = new[]
        {
            new Product("X", 1.00m),
            new Product("Y", 2.00m)
        };

        // Act & Assert
        var exception = Should.Throw<InvalidOperationException>(() => terminal.SetPricing(newProducts));
        exception.Message.ShouldBe("Cannot change pricing during active transaction. Call Clear() first.");
    }
}
