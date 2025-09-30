namespace PosTerminal.Models;

/// <summary>
/// Represents a product with its pricing information including optional volume discounts.
/// </summary>
public sealed class Product
{
    /// <summary>
    /// Gets the unique product code identifier.
    /// </summary>
    public string Code { get; }

    /// <summary>
    /// Gets the regular price per unit.
    /// </summary>
    public decimal UnitPrice { get; }

    /// <summary>
    /// Gets the volume pricing information if available.
    /// </summary>
    public VolumePricing? VolumePricing { get; }

    /// <summary>
    /// Initializes a new instance of the Product class.
    /// </summary>
    /// <param name="code">The unique product code.</param>
    /// <param name="unitPrice">The price per unit.</param>
    /// <param name="volumePricing">Optional volume pricing information.</param>
    /// <exception cref="ArgumentException">Thrown when code is null or empty, or unitPrice is negative.</exception>
    public Product(string code, decimal unitPrice, VolumePricing? volumePricing = null)
    {
        EnsureValidArguments(code, unitPrice);

        Code = code;
        UnitPrice = unitPrice;
        VolumePricing = volumePricing;
    }

    private static void EnsureValidArguments(string code, decimal unitPrice)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Product code cannot be null or empty.", nameof(code));

        if (unitPrice < 0)
            throw new ArgumentException("Unit price cannot be negative.", nameof(unitPrice));
    }
}
