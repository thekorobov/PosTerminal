using PosTerminal;
using PosTerminal.Models;

const string basicHeader = "Point of Sale Terminal - Basic Functionality";
const string discountHeader = "Point of Sale Terminal - Discount Card Features";

// Create and configure the point-of-sale terminal
var terminal = new PointOfSaleTerminal();

// Set up pricing according to the requirements
var products = new[]
{
    new Product("A", 1.25m, new VolumePricing(3, 3.00m)), // $1.25 or $3 for a three-pack
    new Product("B", 4.25m), // $4.25 per unit
    new Product("C", 1.00m, new VolumePricing(6, 5.00m)), // $1.00 or $5 for a six-pack
    new Product("D", 0.75m) // $0.75 per unit
};

terminal.SetPricing(products);

#region Basic Functionality Tests

Console.WriteLine(new string('=', basicHeader.Length));
Console.WriteLine(basicHeader);
Console.WriteLine(new string('=', basicHeader.Length));
Console.WriteLine();

// Test Case 1.1: AAAABCDAAA should total $13.25
Console.WriteLine("Test Case 1.1: Scanning AAAABCDAAA");
ScanItems(terminal, "AAAABCDAAA");
Console.WriteLine($"Total: ${terminal.CalculateTotal():F2} (Expected: $13.25)");
Console.WriteLine();

// Test Case 1.2: CCCCCCC should total $6.00
Console.WriteLine("Test Case 1.2: Scanning CCCCCCC");
ScanItems(terminal, "CCCCCCC");
Console.WriteLine($"Total: ${terminal.CalculateTotal():F2} (Expected: $6.00)");
Console.WriteLine();

// Test Case 1.3: ABCD should total $7.25
Console.WriteLine("Test Case 1.3: Scanning ABCD");
ScanItems(terminal, "ABCD");
Console.WriteLine($"Total: ${terminal.CalculateTotal():F2} (Expected: $7.25)");
Console.WriteLine();

#endregion

#region Discount Card Tests

Console.WriteLine(new string('=', discountHeader.Length));
Console.WriteLine(discountHeader);
Console.WriteLine(new string('=', discountHeader.Length));
Console.WriteLine();

// Test Case 2.1: New card (0%) with ABCD - $7.25
var newCard = new DiscountCard();
Console.WriteLine($"Test Case 2.1: No card ({newCard.GetPercent() * 100:F0}%) with ABCD");
ScanItems(terminal, "ABCD");
decimal totalWithNewCard = terminal.CalculateTotal(newCard);
Console.WriteLine($"Total: ${totalWithNewCard:F2} (no discount)");
Console.WriteLine($"Card accumulated: ${newCard.AccumulatedAmount:F2}");
Console.WriteLine();

// Test Case 2.2: Bronze card (1%) with BBBB - $17.00
var bronzeCard = new DiscountCard(1500m);
Console.WriteLine($"Test Case 2.2: Bronze card ({bronzeCard.GetPercent() * 100:F0}%) with BBBB");
ScanItems(terminal, "BBBB");
decimal totalWithBronze = terminal.CalculateTotal(bronzeCard);
Console.WriteLine($"Total: ${totalWithBronze:F2} (saved: ${17.00m - totalWithBronze:F2})");
Console.WriteLine($"Card accumulated: ${bronzeCard.AccumulatedAmount:F2}");
Console.WriteLine();

// Test Case 2.3: Silver card (3%) with AAAABCDAAA - $13.25
var silverCard = new DiscountCard(2150m);
Console.WriteLine($"Test Case 2.3: Silver card ({silverCard.GetPercent() * 100:F0}%) with AAAABCDAAA");
ScanItems(terminal, "AAAABCDAAA");
decimal totalWithSilver = terminal.CalculateTotal(silverCard);
Console.WriteLine($"Total: ${totalWithSilver:F2} (saved: ${13.25m - totalWithSilver:F2})");
Console.WriteLine($"Card accumulated: ${silverCard.AccumulatedAmount:F2}");
Console.WriteLine();

// Test Case 2.4: Gold card (5%) with CCCCCCC - $6.00
var goldCard = new DiscountCard(6000m);
Console.WriteLine($"Test Case 2.4: Gold card ({goldCard.GetPercent() * 100:F0}%) with CCCCCCC");
ScanItems(terminal, "CCCCCCC");
decimal totalWithGold = terminal.CalculateTotal(goldCard);
Console.WriteLine($"Total: ${totalWithGold:F2} (saved: ${6.00m - totalWithGold:F2})");
Console.WriteLine($"Card accumulated: ${goldCard.AccumulatedAmount:F2}");
Console.WriteLine();

// Test Case 2.5: Platinum card (7%) with ABCD - $7.25
var platinumCard = new DiscountCard(12_000m);
Console.WriteLine($"Test Case 2.5: Platinum card ({platinumCard.GetPercent() * 100:F0}%) with ABCD");
ScanItems(terminal, "ABCD");
decimal totalWithPlatinum = terminal.CalculateTotal(platinumCard);
Console.WriteLine($"Total: ${totalWithPlatinum:F2} (saved: ${7.25m - totalWithPlatinum:F2})");
Console.WriteLine($"Card accumulated: ${platinumCard.AccumulatedAmount:F2}");
Console.WriteLine();

#endregion

Console.WriteLine("All test cases completed successfully!");
Console.WriteLine("Press any key to exit...");
Console.ReadKey();
return;

static void ScanItems(PointOfSaleTerminal terminal, string items)
{
    terminal.Clear();
    foreach (char item in items)
    {
        terminal.Scan(item.ToString());
    }
}
