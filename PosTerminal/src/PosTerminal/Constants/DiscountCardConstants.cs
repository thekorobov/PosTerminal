namespace PosTerminal.Constants;

/// <summary>
/// Contains thresholds and discount rates for discount card levels.
/// </summary>
public static class DiscountCardConstants
{
    // Bronze Level
    public const decimal BronzeThreshold = 1000m;
    public const decimal BronzePercent = 0.01m;

    // Silver Level  
    public const decimal SilverThreshold = 2000m;
    public const decimal SilverPercent = 0.03m;

    // Gold Level
    public const decimal GoldThreshold = 5000m;
    public const decimal GoldPercent = 0.05m;

    // Platinum Level
    public const decimal PlatinumThreshold = 10_000m;
    public const decimal PlatinumPercent = 0.07m;

    // No Card
    public const decimal NoCardPercent = 0.00m;
}
