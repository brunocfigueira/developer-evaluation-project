using Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData;

public static class UpdateProductHandlerTestData
{
    private static readonly Faker<UpdateProductCommand> commandFaker = new Faker<UpdateProductCommand>()
        .RuleFor(c => c.Id, f => f.Random.Int(1, 1000))
        .RuleFor(c => c.Title, f => f.Commerce.ProductName())
        .RuleFor(c => c.Description, f => f.Commerce.ProductDescription())
        .RuleFor(c => c.Price, f => f.Random.Decimal(1, 1000))
        .RuleFor(c => c.Category, f => f.Commerce.Categories(1)[0])
        .RuleFor(c => c.Image, f => f.Image.PicsumUrl());

    private static readonly Faker<Product> productFaker = new Faker<Product>()
        .RuleFor(p => p.Id, f => f.Random.Int(1, 1000))
        .RuleFor(p => p.Title, f => f.Commerce.ProductName())
        .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
        .RuleFor(p => p.Price, f => f.Random.Decimal(1, 1000))
        .RuleFor(p => p.Category, f => f.Commerce.Categories(1)[0])
        .RuleFor(p => p.Image, f => f.Image.PicsumUrl())
        .RuleFor(p => p.CreatedAt, f => f.Date.Past())
        .RuleFor(p => p.UpdatedAt, f => f.Date.Recent());

    public static UpdateProductCommand GenerateValidCommand() => commandFaker.Generate();

    public static Product GenerateProductFromCommand(UpdateProductCommand command)
    {
        var product = productFaker.Generate();
        product.Id = command.Id;
        product.Title = command.Title;
        product.Description = command.Description;
        product.Price = command.Price;
        product.Category = command.Category;
        product.Image = command.Image;
        return product;
    }
}
