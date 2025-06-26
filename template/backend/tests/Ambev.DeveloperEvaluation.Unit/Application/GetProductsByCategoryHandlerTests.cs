using Ambev.DeveloperEvaluation.Application.Products.GetProducts;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class GetProductsByCategoryHandlerTests
{
    private readonly IProductRepository _productRepository;
    private readonly GetProductsByCategoryHandler _handler;

    public GetProductsByCategoryHandlerTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _handler = new GetProductsByCategoryHandler(_productRepository);
    }

    [Fact(DisplayName = "Given products exist in category When getting products Then returns products and total count")]
    public async Task Handle_ProductsExistInCategory_ReturnsProductsAndTotalCount()
    {
        // Arrange
        var category = "Bebidas";
        var (products, totalCount) = GetProductsByCategoryHandlerTestData.GenerateProductsByCategory(5, category);
        var query = new GetProductsByCategoryCommand { Category = category, Page = 1, PageSize = 10, Order = null };
        _productRepository.GetByCategoryAsync(category, 1, 10, null, Arg.Any<CancellationToken>()).Returns((products, totalCount));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Items.Should().BeEquivalentTo(products);
        result.TotalCount.Should().Be(totalCount);
        await _productRepository.Received(1).GetByCategoryAsync(category, 1, 10, null, Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Given no products in category When getting products Then returns empty list and zero total count")]
    public async Task Handle_NoProductsInCategory_ReturnsEmptyListAndZeroTotalCount()
    {
        // Arrange
        var category = "Vazios";
        var products = new List<Product>();
        var query = new GetProductsByCategoryCommand { Category = category, Page = 1, PageSize = 10, Order = null };
        _productRepository.GetByCategoryAsync(category, 1, 10, null, Arg.Any<CancellationToken>()).Returns((products, 0));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Items.Should().BeEmpty();
        result.TotalCount.Should().Be(0);
        await _productRepository.Received(1).GetByCategoryAsync(category, 1, 10, null, Arg.Any<CancellationToken>());
    }
}
