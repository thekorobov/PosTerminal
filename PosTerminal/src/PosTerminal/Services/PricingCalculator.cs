using PosTerminal.Models;

namespace PosTerminal.Services;

/// <summary>
/// Provides pricing calculations for products including volume discount optimization.
/// </summary>
public sealed class PricingCalculator
{
    /// <summary>
    /// Calculates detailed pricing breakdown including information for discount card processing.
    /// </summary>
    /// <param name="product">The product to calculate pricing for.</param>
    /// <param name="quantity">The quantity being purchased.</param>
    /// <returns>Detailed breakdown of pricing components.</returns>
    /// <exception cref="ArgumentNullException">Thrown when product is null.</exception>
    /// <exception cref="ArgumentException">Thrown when quantity is negative.</exception>
    public PricingBreakdown CalculateBreakdown(Product product, int quantity)
    {
        EnsureValidArguments(product, quantity);
        if (quantity == 0) return new PricingBreakdown(0m, 0m, 0m);

        decimal gross = CalculateGrossAmount(product, quantity);

        return HasVolumePricing(product)
            ? CalculateBreakdownWithVolume(product, quantity, gross)
            : CalculateBreakdownNoVolume(product, quantity, gross);
    }

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

    private static PricingBreakdown CalculateBreakdownNoVolume(Product product, int quantity, decimal gross)
    {
        decimal unitTotal = CalculateUnitOnly(product, quantity);
        return new PricingBreakdown(
            TotalPrice: unitTotal,
            CardEligibleAmount: unitTotal,
            GrossAmount: gross
        );
    }

    private static PricingBreakdown CalculateBreakdownWithVolume(Product product, int quantity, decimal gross)
    {
        var pricing = product.VolumePricing!;
        var (packs, remainder) = SplitIntoPacks(quantity, pricing.Quantity);

        decimal packsTotal = packs * pricing.Price;
        decimal remainderTotal = remainder * product.UnitPrice;

        return new PricingBreakdown(
            TotalPrice: packsTotal + remainderTotal,
            CardEligibleAmount: remainderTotal,
            GrossAmount: gross
        );
    }

    private static decimal CalculateUnitOnly(Product product, int quantity)
        => product.UnitPrice * quantity;

    private static decimal CalculateWithVolume(Product product, int quantity)
    {
        var pricing = product.VolumePricing!;
        var (packs, remainder) = SplitIntoPacks(quantity, pricing.Quantity);
        return packs * pricing.Price + remainder * product.UnitPrice;
    }

    private static void EnsureValidArguments(Product product, int quantity)
    {
        ArgumentNullException.ThrowIfNull(product);
        if (quantity < 0) throw new ArgumentException("Quantity cannot be negative.", nameof(quantity));
    }

    private static bool HasVolumePricing(Product product)
        => product.VolumePricing is not null;

    private static (int packs, int remainder) SplitIntoPacks(int quantity, int packSize)
        => (quantity / packSize, quantity % packSize);

    private static decimal CalculateGrossAmount(Product product, int quantity)
        => product.UnitPrice * quantity;
}
