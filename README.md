# Point of Sale Terminal

A .NET 9.0 implementation of a point-of-sale terminal with support for unit and volume pricing.

## 📋 Problem Statement

Implement a point-of-sale API that allows scanning products in any order and calculates the total price. Each product may have a single volume discount rule.

## 🛍️ Product Catalog

| Product Code | Pricing                   |
| ------------ | ------------------------- |
| A            | $1.25 each or 3 for $3.00 |
| B            | $4.25 each                |
| C            | $1.00 each or 6 for $5.00 |
| D            | $0.75 each                |

## 🚀 Getting Started

### Prerequisites

- .NET 9.0 SDK
- Any compatible IDE (Visual Studio, Rider, VS Code)

### Build

```bash
dotnet build
```

### Run Tests

```bash
dotnet test
```

### Run Demo

```bash
dotnet run --project src/PosTerminal
```

## 💡 Usage Example

```csharp
var terminal = new PointOfSaleTerminal();

var products = new[]
{
    new Product("A", 1.25m, new VolumePricing(3, 3.00m)),
    new Product("B", 4.25m),
    new Product("C", 1.00m, new VolumePricing(6, 5.00m)),
    new Product("D", 0.75m)
};
terminal.SetPricing(products);

terminal.Scan("A");
terminal.Scan("B");
terminal.Scan("A");

decimal total = terminal.CalculateTotal(); // returns 6.75
```

## ✅ Test Cases

The following scenarios are validated:

| Input        | Expected Total |
| ------------ | -------------- |
| `AAAABCDAAA` | $13.25         |
| `CCCCCCC`    | $6.00          |
| `ABCD`       | $7.25          |

### Calculation Example

**AAAABCDAAA → $13.25**

- A: 7 items = 2 × (3 for $3.00) + 1 × $1.25 = $7.25
- B: 1 item = $4.25
- C: 1 item = $1.00
- D: 1 item = $0.75
- **Total = $13.25**

## 🏗️ Project Structure

```
src/
└── PosTerminal/
    ├── Models/                # Product, VolumePricing
    ├── Services/              # PricingCalculator
    ├── PointOfSaleTerminal.cs # Main API
    └── Program.cs             # Demo
└── PosTerminal.Console/
    └── Program.cs             # Demo
tests/
└── PosTerminal.UnitTests/     # Unit tests
```

## 🔑 Features

- Unit and volume pricing with optimal calculation
- Clear input validation and error handling
- End-to-end tests verifying required scenarios
