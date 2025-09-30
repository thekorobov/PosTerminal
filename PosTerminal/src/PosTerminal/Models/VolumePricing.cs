namespace PosTerminal.Models;

/// <summary>
/// Represents volume pricing information for bulk purchases.
/// </summary>
public sealed class VolumePricing
{
    /// <summary>
    /// The minimum quantity required for volume discount.
    /// </summary>
    private const int MinimumVolumeQuantity = 2;

    /// <summary>
    /// Gets the minimum quantity required for the volume discount.
    /// </summary>
    public int Quantity { get; }

    /// <summary>
    /// Gets the total price for the specified quantity.
    /// </summary>
    public decimal Price { get; }

    /// <summary>
    /// Initializes a new instance of the VolumePricing class.
    /// </summary>
    /// <param name="quantity">The minimum quantity for volume discount.</param>
    /// <param name="price">The total price for the specified quantity.</param>
    /// <exception cref="ArgumentException">Thrown when quantity is less than 2 or price is negative.</exception>
    public VolumePricing(int quantity, decimal price)
    {
        EnsureValidArguments(quantity, price);

        Quantity = quantity;
        Price = price;
    }

    private static void EnsureValidArguments(int quantity, decimal price)
    {
        if (quantity < MinimumVolumeQuantity)
            throw new ArgumentException($"Volume pricing quantity must be at least {MinimumVolumeQuantity}.", nameof(quantity));

        if (price < 0)
            throw new ArgumentException("Volume pricing price cannot be negative.", nameof(price));
    }
}
