using Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class DeleteProductHandlerTests
{
    private readonly IProductRepository _productRepository;
    private readonly DeleteProductHandler _handler;

    public DeleteProductHandlerTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _handler = new DeleteProductHandler(_productRepository);
    }

    [Fact(DisplayName = "Given existing product id When deleting product Then product is deleted")]
    public async Task Handle_ExistingProduct_DeletesProduct()
    {
        // Arrange
        var command = new DeleteProductCommand { Id = 123 };
        _productRepository.ExistsAsync(command.Id, Arg.Any<CancellationToken>()).Returns(true);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _productRepository.Received(1).DeleteAsync(command.Id, Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Given non-existing product id When deleting product Then throws ResourceNotFoundException")]
    public async Task Handle_NonExistingProduct_ThrowsResourceNotFoundException()
    {
        // Arrange
        var command = new DeleteProductCommand { Id = 123 };
        _productRepository.ExistsAsync(command.Id, Arg.Any<CancellationToken>()).Returns(false);

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ResourceNotFoundException>()
            .WithMessage($"Product with ID {command.Id} not found");
        await _productRepository.DidNotReceive().DeleteAsync(Arg.Any<int>(), Arg.Any<CancellationToken>());
    }
}