using PosTerminal.Models;
using PosTerminal.Services;

namespace PosTerminal;

/// <summary>
/// Point-of-sale terminal that manages product scanning and total calculation.
/// </summary>
public sealed class PointOfSaleTerminal
{
    private readonly Dictionary<string, Product> _products = new();
    private readonly Dictionary<string, int> _scannedItems = new();
    private readonly PricingCalculator _pricingCalculator = new();

    /// <summary>
    /// Sets the pricing configuration for all products.
    /// </summary>
    /// <param name="products">The products to configure in the system.</param>
    /// <exception cref="ArgumentNullException">Thrown when products' collection is null.</exception>
    /// <exception cref="ArgumentException">Thrown when products' collection is empty or contains duplicate codes.</exception>
    /// <exception cref="InvalidOperationException"> Thrown when pricing is changed during an active transaction.</exception>
    public void SetPricing(IEnumerable<Product> products)
    {
        if (_scannedItems.Count > 0)
            throw new InvalidOperationException("Cannot change pricing during active transaction. Call Clear() first.");

        var validatedProducts = ValidateProducts(products);
        var newProductsDictionary = BuildProductDictionary(validatedProducts);

        _products.Clear();
        foreach (var (code, product) in newProductsDictionary)
        {
            _products.Add(code, product);
        }
    }

    /// <summary>
    /// Scans a product by its code, adding it to the current transaction.
    /// </summary>
    /// <param name="productCode">The code of the product to scan.</param>
    /// <exception cref="ArgumentException">Thrown when product code is null or empty.</exception>
    /// <exception cref="InvalidOperationException">Thrown when pricing is not configured or product code is not found.</exception>
    public void Scan(string productCode)
    {
        if (string.IsNullOrWhiteSpace(productCode))
            throw new ArgumentException("Product code cannot be null or empty.", nameof(productCode));

        EnsurePricingConfigured();

        if (!_products.ContainsKey(productCode))
            throw new InvalidOperationException($"Product code '{productCode}' not found.");

        IncrementQuantity(productCode);
    }

    /// <summary>
    /// Calculates the total price for all scanned items, applying volume discounts where applicable.
    /// </summary>
    /// <returns>The total price as a decimal value.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no pricing has been set.</exception>
    public decimal CalculateTotal()
    {
        EnsurePricingConfigured();
        if (_scannedItems.Count == 0) return 0m;

        decimal total = 0m;
        foreach (var (code, quantity) in _scannedItems)
        {
            var product = _products[code];
            total += _pricingCalculator.CalculatePrice(product, quantity);
        }

        return total;
    }

    /// <summary>
    /// Clears all scanned items from the current transaction to start a new transaction.
    /// </summary>
    public void Clear() => _scannedItems.Clear();

    private static List<Product> ValidateProducts(IEnumerable<Product> products)
    {
        ArgumentNullException.ThrowIfNull(products);

        var list = products as List<Product> ?? products.ToList();
        if (list.Count == 0)
            throw new ArgumentException("Products collection cannot be empty.", nameof(products));

        return list;
    }

    private static Dictionary<string, Product> BuildProductDictionary(IEnumerable<Product> products)
    {
        var productDictionary = new Dictionary<string, Product>();

        foreach (var product in products)
        {
            if (!productDictionary.TryAdd(product.Code, product))
                throw new ArgumentException($"Duplicate product code found: {product.Code}", nameof(products));
        }

        return productDictionary;
    }

    private void EnsurePricingConfigured()
    {
        if (_products.Count == 0)
            throw new InvalidOperationException("No pricing configuration set. Call SetPricing first.");
    }

    private void IncrementQuantity(string code) => _scannedItems[code] = _scannedItems.GetValueOrDefault(code) + 1;
}

