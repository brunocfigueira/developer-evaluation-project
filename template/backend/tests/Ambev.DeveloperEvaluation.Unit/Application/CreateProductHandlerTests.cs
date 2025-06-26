using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentAssertions;
using NSubstitute;
using Xunit;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class CreateProductHandlerTests
{
    private readonly IProductRepository _productRepository;
    private readonly CreateProductHandler _handler;

    public CreateProductHandlerTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _handler = new CreateProductHandler(_productRepository);
    }

    [Fact(DisplayName = "Given valid product data When creating product Then returns product id")]
    public async Task Handle_ValidRequest_ReturnsProductId()
    {
        // Arrange
        var command = CreateProductHandlerTestData.GenerateValidCommand();
        var product = CreateProductHandlerTestData.GenerateProductFromCommand(command, 123);
        _productRepository.AddAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>()).Returns(product);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(product.Id);
        await _productRepository.Received(1).AddAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Given invalid product data When creating product Then throws validation exception")]
    public async Task Handle_InvalidRequest_ThrowsValidationException()
    {
        // Arrange
        var command = new CreateProductCommand(); // Invalid: missing required fields

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }

    [Fact(DisplayName = "Given valid product data When creating product Then product is persisted with correct data")]
    public async Task Handle_ValidRequest_PersistsProductWithCorrectData()
    {
        // Arrange
        var command = CreateProductHandlerTestData.GenerateValidCommand();
        Product? capturedProduct = null;
        _productRepository.AddAsync(Arg.Do<Product>(p => capturedProduct = p), Arg.Any<CancellationToken>())
            .Returns(ci =>
            {
                capturedProduct!.Id = 99;
                return capturedProduct;
            });

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        capturedProduct.Should().NotBeNull();
        capturedProduct!.Title.Should().Be(command.Title);
        capturedProduct.Description.Should().Be(command.Description);
        capturedProduct.Price.Should().Be(command.Price);
        capturedProduct.Category.Should().Be(command.Category);
        capturedProduct.Image.Should().Be(command.Image);
        result.Should().Be(99);
    }
}
