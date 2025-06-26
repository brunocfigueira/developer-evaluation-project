using Ambev.DeveloperEvaluation.Application.Products.GetProducts;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class GetProductsHandlerTests
{
    private readonly IProductRepository _productRepository;
    private readonly GetProductsHandler _handler;

    public GetProductsHandlerTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _handler = new GetProductsHandler(_productRepository);
    }

    [Fact(DisplayName = "Given products exist When getting products Then returns products and total count")]
    public async Task Handle_ProductsExist_ReturnsProductsAndTotalCount()
    {
        // Arrange
        var (products, totalCount) = GetProductsHandlerTestData.GenerateProducts(5);
        var query = new GetProductsCommand { Page = 1, PageSize = 10, Order = null };
        _productRepository.GetAllAsync(1, 10, null, Arg.Any<CancellationToken>()).Returns((products, totalCount));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Items.Should().BeEquivalentTo(products);
        result.TotalCount.Should().Be(totalCount);
        await _productRepository.Received(1).GetAllAsync(1, 10, null, Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Given no products exist When getting products Then returns empty list and zero total count")]
    public async Task Handle_NoProductsExist_ReturnsEmptyListAndZeroTotalCount()
    {
        // Arrange
        var products = new List<Product>();
        var query = new GetProductsCommand { Page = 1, PageSize = 10, Order = null };
        _productRepository.GetAllAsync(1, 10, null, Arg.Any<CancellationToken>()).Returns((products, 0));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Items.Should().BeEmpty();
        result.TotalCount.Should().Be(0);
        await _productRepository.Received(1).GetAllAsync(1, 10, null, Arg.Any<CancellationToken>());
    }
}
