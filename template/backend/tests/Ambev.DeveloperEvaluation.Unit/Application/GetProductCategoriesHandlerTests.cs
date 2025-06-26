using Ambev.DeveloperEvaluation.Application.Products.GetProducts;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class GetProductCategoriesHandlerTests
{
    private readonly IProductRepository _productRepository;
    private readonly GetProductCategoriesHandler _handler;

    public GetProductCategoriesHandlerTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _handler = new GetProductCategoriesHandler(_productRepository);
    }

    [Fact(DisplayName = "Given categories exist When getting categories Then returns categories list")]
    public async Task Handle_CategoriesExist_ReturnsCategories()
    {
        // Arrange
        var categories = GetProductCategoriesHandlerTestData.GenerateCategories(5);
        _productRepository.GetCategoriesAsync(Arg.Any<CancellationToken>()).Returns(categories);
        var query = new GetProductCategoriesCommand();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(categories);
        await _productRepository.Received(1).GetCategoriesAsync(Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Given no categories exist When getting categories Then returns empty list")]
    public async Task Handle_NoCategoriesExist_ReturnsEmptyList()
    {
        // Arrange
        var categories = new List<string>();
        _productRepository.GetCategoriesAsync(Arg.Any<CancellationToken>()).Returns(categories);
        var query = new GetProductCategoriesCommand();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
        await _productRepository.Received(1).GetCategoriesAsync(Arg.Any<CancellationToken>());
    }
}
