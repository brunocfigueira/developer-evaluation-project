using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData;

public static class GetProductsByCategoryHandlerTestData
{
    private static readonly Faker<Product> productFaker = new Faker<Product>()
        .RuleFor(p => p.Id, f => f.Random.Int(1, 1000))
        .RuleFor(p => p.Title, f => f.Commerce.ProductName())
        .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
        .RuleFor(p => p.Price, f => f.Random.Decimal(1, 1000))
        .RuleFor(p => p.Category, f => f.Commerce.Categories(1)[0])
        .RuleFor(p => p.Image, f => f.Image.PicsumUrl())
        .RuleFor(p => p.CreatedAt, f => f.Date.Past())
        .RuleFor(p => p.UpdatedAt, f => f.Date.Recent());

    public static (List<Product> Items, int TotalCount) GenerateProductsByCategory(int count = 3, string? category = null)
    {
        var products = productFaker.Generate(count);
        if (!string.IsNullOrEmpty(category))
            products.ForEach(p => p.Category = category);
        return (products, products.Count);
    }
}
