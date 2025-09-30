using PosTerminal.Models;

namespace PosTerminal.Services;

/// <summary>
/// Provides pricing calculations for products including volume discount optimization.
/// </summary>
public sealed class PricingCalculator
{
    /// <summary>
    /// Calculates the optimal price for a given quantity of a product.
    /// </summary>
    /// <param name="product">The product to calculate pricing for.</param>
    /// <param name="quantity">The quantity being purchased.</param>
    /// <returns>The optimal total price considering volume discounts.</returns>
    /// <exception cref="ArgumentNullException">Thrown when product is null.</exception>
    /// <exception cref="ArgumentException">Thrown when quantity is negative.</exception>
    public decimal CalculatePrice(Product product, int quantity)
    {
        EnsureValidArguments(product, quantity);
        if (quantity == 0) return 0m;

        return HasVolumePricing(product)
            ? CalculateWithVolume(product, quantity)
            : CalculateUnitOnly(product, quantity);
    }

    private static void EnsureValidArguments(Product product, int quantity)
    {
        ArgumentNullException.ThrowIfNull(product);

        if (quantity < 0)
            throw new ArgumentException("Quantity cannot be negative.", nameof(quantity));
    }

    private static bool HasVolumePricing(Product product)
        => product.VolumePricing is not null;

    private static decimal CalculateUnitOnly(Product product, int quantity)
        => product.UnitPrice * quantity;

    private static decimal CalculateWithVolume(Product product, int quantity)
    {
        var pricing = product.VolumePricing!;
        var (packs, remainder) = SplitIntoPacks(quantity, pricing.Quantity);
        return packs * pricing.Price + remainder * product.UnitPrice;
    }

    private static (int packs, int remainder) SplitIntoPacks(int quantity, int packSize)
        => (quantity / packSize, quantity % packSize);
}
