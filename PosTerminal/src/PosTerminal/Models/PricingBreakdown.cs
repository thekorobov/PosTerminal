namespace PosTerminal.Models;

/// <summary>
/// Represents the detailed pricing breakdown for a product calculation.
/// </summary>
/// <remarks>
/// This structure is used internally for discount card processing calculations.
/// It provides information about which portions of the total are eligible for card discounts.
/// </remarks>
/// <param name="TotalPrice">The total price after applying volume discounts, but before card discount.</param>
/// <param name="CardEligibleAmount">The portion of the total eligible for discount card percentage.</param>
/// <param name="GrossAmount">The gross sale amount without any discounts applied.</param>
public readonly record struct PricingBreakdown(
    decimal TotalPrice,
    decimal CardEligibleAmount,
    decimal GrossAmount
);
