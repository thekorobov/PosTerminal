# Point of Sale Terminal

A .NET 9.0 implementation of a point-of-sale terminal with support for unit and volume pricing, plus customer discount cards.

## 📋 Problem Statement

This project implements a complete point-of-sale system in two parts:

**Part 1**: Basic terminal functionality that allows scanning products in any order and calculates the total price with optimal volume discounts.

**Part 2**: Customer discount card system with accumulated spending thresholds and percentage discounts that apply only to items not already discounted by volume pricing.

## 🛍️ Product Catalog

| Product Code | Pricing                   |
| ------------ | ------------------------- |
| A            | $1.25 each or 3 for $3.00 |
| B            | $4.25 each                |
| C            | $1.00 each or 6 for $5.00 |
| D            | $0.75 each                |

## 💳 Discount Card Levels

| Card Level | Accumulated Spend | Discount Rate |
| ---------- | ----------------- | ------------- |
| No Card    | $0                | 0%            |
| Bronze     | $1,000-$1,999     | 1%            |
| Silver     | $2,000-$4,999     | 3%            |
| Gold       | $5,000-$9,999     | 5%            |
| Platinum   | over $9,999       | 7%            |

**Note**: Discount cards apply only to items sold at unit price (not volume-discounted items).

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
dotnet run --project src/PosTerminal.Console
```

## 💡 Usage Examples

### Basic Usage

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

### Usage with Discount Card

```csharp
var terminal = new PointOfSaleTerminal();
// ... set pricing as above

var silverCard = new DiscountCard(2150m); // Silver level (3% discount)

// Scanning AAAABCDAAA
foreach (char item in "AAAABCDAAA")
{
    terminal.Scan(item.ToString());
}

decimal total = terminal.CalculateTotal(silverCard);
// Result: $13.0325 (see calculation example below)
// Card accumulated amount increases by $14.75 (gross amount)
```

## ✅ Test Cases

### Part 1: Basic Functionality

| Input        | Expected Total |
| ------------ | -------------- |
| `AAAABCDAAA` | $13.25         |
| `CCCCCCC`    | $6.00          |
| `ABCD`       | $7.25          |

### Calculation Example

#### AAAABCDAAA → $13.25

- A: 7 items = 2 × (3 for $3.00) + 1 × $1.25 = $7.25
- B: 1 item = $4.25
- C: 1 item = $1.00
- D: 1 item = $0.75
- **Total = $13.25**

### Part 2: Discount Card Scenarios

| Card Level | Items        | Before Discount | Card Discount | Final Total |
| ---------- | ------------ | --------------- | ------------- | ----------- |
| No Card    | `ABCD`       | $7.25           | $0.00         | $7.25       |
| Bronze     | `BBBB`       | $17.00          | $0.17         | $16.83      |
| Silver     | `AAAABCDAAA` | $13.25          | $0.2175       | $13.0325    |
| Gold       | `CCCCCCC`    | $6.00           | $0.05         | $5.95       |
| Platinum   | `ABCD`       | $7.25           | $0.5075       | $6.7425     |

### Calculation Example (Discount Cards)

#### Silver Card with AAAABCDAAA → $13.0325

- **Volume discounts applied first**:
  - A: 7 items → 2 volume packs ($6.00) + 1 unit ($1.25) = $7.25
  - B: 1 item → $4.25 (eligible for card discount)
  - C: 1 item → $1.00 (eligible for card discount)
  - D: 1 item → $0.75 (eligible for card discount)
- **Card discount (3%) on eligible items**: ($1.25 + $4.25 + $1.00 + $0.75) × 0.03 = $0.2175
- **Final total**: $13.25 - $0.2175 = $13.0325
- **Card accumulation**: Gross amount $14.75 (7 × $1.25 + $4.25 + $1.00 + $0.75)

## 🏗️ Project Structure

```text
PosTerminal/
├── src/
│   ├── PosTerminal/
│   │   ├── Constants/
│   │   │   └── DiscountCardConstants.cs    # Card level thresholds
│   │   ├── Models/
│   │   │   ├── DiscountCard.cs             # Customer discount card
│   │   │   ├── PricingBreakdown.cs         # Detailed price breakdown
│   │   │   ├── Product.cs                  # Product definition
│   │   │   └── VolumePricing.cs            # Volume discount rules
│   │   ├── Services/
│   │   │   └── PricingCalculator.cs        # Price calculation logic
│   │   └── PointOfSaleTerminal.cs          # Main API
│   └── PosTerminal.Console/
│       └── Program.cs                      # Demo application
└── tests/
    └── PosTerminal.UnitTests/
        ├── Models/                         # Model tests
        ├── Services/                       # Service tests
        └── PointOfSaleTerminalTests.cs     # End-to-end tests
```

## 🔑 Features

### Part 1: Core Terminal Functionality

- Unit and volume pricing with optimal calculation
- Automatic application of best volume discounts
- Clear input validation and error handling

### Part 2: Discount Card System

- Four customer card levels (Bronze, Silver, Gold, Platinum)
- Percentage discounts based on accumulated spending
- Card discounts apply only to non-volume-discounted items
- Automatic accumulation of gross sales to customer cards
