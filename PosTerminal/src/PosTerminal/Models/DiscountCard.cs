using PosTerminal.Constants;

namespace PosTerminal.Models;

/// <summary>
/// Represents a customer discount card that tracks accumulated gross spend
/// and provides a discount rate for purchases without other applied discounts.
/// </summary>
public sealed class DiscountCard
{
    /// <summary>
    /// Total gross amount accumulated on the card (sales before discounts).
    /// </summary>
    public decimal AccumulatedAmount { get; private set; }

    public DiscountCard(decimal initialAmount = 0m)
    {
        if (initialAmount < 0) throw new ArgumentException("Initial amount cannot be negative.", nameof(initialAmount));
        AccumulatedAmount = initialAmount;
    }

    /// <summary>
    /// Returns the current discount rate based on the accumulated amount.
    /// </summary>
    public decimal GetPercent()
        => AccumulatedAmount switch
        {
            >= DiscountCardConstants.PlatinumThreshold => DiscountCardConstants.PlatinumPercent,
            >= DiscountCardConstants.GoldThreshold => DiscountCardConstants.GoldPercent,
            >= DiscountCardConstants.SilverThreshold => DiscountCardConstants.SilverPercent,
            >= DiscountCardConstants.BronzeThreshold => DiscountCardConstants.BronzePercent,
            _ => DiscountCardConstants.NoCardPercent
        };

    /// <summary>
    /// Adds the gross sale amount (before discounts) to the accumulated total.
    /// </summary>
    public void Accumulate(decimal grossSaleAmount)
    {
        if (grossSaleAmount < 0) throw new ArgumentException("Gross sale cannot be negative.", nameof(grossSaleAmount));
        AccumulatedAmount += grossSaleAmount;
    }
}
