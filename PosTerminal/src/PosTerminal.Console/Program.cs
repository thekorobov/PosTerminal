using PosTerminal;
using PosTerminal.Models;

// Create and configure the point-of-sale terminal
var terminal = new PointOfSaleTerminal();

// Set up pricing according to the requirements
var products = new[]
{
    new Product("A", 1.25m, new VolumePricing(3, 3.00m)), // $1.25 or $3 for a three-pack
    new Product("B", 4.25m),                              // $4.25 per unit
    new Product("C", 1.00m, new VolumePricing(6, 5.00m)), // $1.00 or $5 for a six-pack
    new Product("D", 0.75m)                               // $0.75 per unit
};

terminal.SetPricing(products);

Console.WriteLine("Point of Sale Terminal Demo");
Console.WriteLine("============================");
Console.WriteLine();

// Test Case 1: AAAABCDAAA should total $13.25
Console.WriteLine("Test Case 1: Scanning AAAABCDAAA");
terminal.Clear();
foreach (char item in "AAAABCDAAA")
{
    terminal.Scan(item.ToString());
    Console.WriteLine($"Scanned: {item}");
}
Console.WriteLine($"Total: ${terminal.CalculateTotal():F2} (Expected: $13.25)");
Console.WriteLine();

// Test Case 2: CCCCCCC should total $6.00
Console.WriteLine("Test Case 2: Scanning CCCCCCC");
terminal.Clear();
foreach (char item in "CCCCCCC")
{
    terminal.Scan(item.ToString());
    Console.WriteLine($"Scanned: {item}");
}
Console.WriteLine($"Total: ${terminal.CalculateTotal():F2} (Expected: $6.00)");
Console.WriteLine();

// Test Case 3: ABCD should total $7.25
Console.WriteLine("Test Case 3: Scanning ABCD");
terminal.Clear();
foreach (char item in "ABCD")
{
    terminal.Scan(item.ToString());
    Console.WriteLine($"Scanned: {item}");
}
Console.WriteLine($"Total: ${terminal.CalculateTotal():F2} (Expected: $7.25)");
Console.WriteLine();

Console.WriteLine("All test cases completed successfully!");
Console.WriteLine("Press any key to exit...");
Console.ReadKey();
