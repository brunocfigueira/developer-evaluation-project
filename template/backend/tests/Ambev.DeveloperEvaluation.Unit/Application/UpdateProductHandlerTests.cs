using Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class UpdateProductHandlerTests
{
    private readonly IProductRepository _productRepository;
    private readonly UpdateProductHandler _handler;

    public UpdateProductHandlerTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _handler = new UpdateProductHandler(_productRepository);
    }

    [Fact(DisplayName = "Given valid update command When handling Then updates product successfully")]
    public async Task Handle_ValidCommand_UpdatesProduct()
    {
        // Arrange
        var command = UpdateProductHandlerTestData.GenerateValidCommand();
        var product = UpdateProductHandlerTestData.GenerateProductFromCommand(command);
        _productRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns(product);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _productRepository.Received(1).UpdateAsync(Arg.Is<Product>(p =>
            p.Id == command.Id &&
            p.Title == command.Title &&
            p.Description == command.Description &&
            p.Price == command.Price &&
            p.Category == command.Category &&
            p.Image == command.Image &&
            p.UpdatedAt.HasValue), Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Given non-existing product id When updating Then throws ResourceNotFoundException")]
    public async Task Handle_NonExistingProduct_ThrowsResourceNotFoundException()
    {
        // Arrange
        var command = UpdateProductHandlerTestData.GenerateValidCommand();
        _productRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns((Product?)null);

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ResourceNotFoundException>()
            .WithMessage($"Product with ID {command.Id} not found");
        await _productRepository.DidNotReceive().UpdateAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Given valid update command When handling Then updates only expected fields")]
    public async Task Handle_ValidCommand_UpdatesOnlyExpectedFields()
    {
        // Arrange
        var command = UpdateProductHandlerTestData.GenerateValidCommand();
        var product = UpdateProductHandlerTestData.GenerateProductFromCommand(command);
        var originalCreatedAt = product.CreatedAt;
        _productRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns(product);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        product.CreatedAt.Should().Be(originalCreatedAt);
        product.UpdatedAt.Should().NotBeNull();
    }
}
