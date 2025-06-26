using Ambev.DeveloperEvaluation.Application.Products.GetProduct;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using FluentAssertions;
using NSubstitute;
using Xunit;
using System.Collections.Generic;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class GetProductByIdHandlerTests
{
    private readonly IProductRepository _productRepository;
    private readonly GetProductByIdHandler _handler;

    public GetProductByIdHandlerTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _handler = new GetProductByIdHandler(_productRepository);
    }

    [Fact(DisplayName = "Given existing product id When getting product Then returns product")]
    public async Task Handle_ExistingProduct_ReturnsProduct()
    {
        // Arrange
        var product = GetProductByIdHandlerTestData.GenerateProduct();
        var command = GetProductByIdHandlerTestData.GenerateQuery(product.Id);
        _productRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns(product);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(product);
        await _productRepository.Received(1).GetByIdAsync(command.Id, Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Given non-existing product id When getting product Then throws KeyNotFoundException")]
    public async Task Handle_NonExistingProduct_ThrowsKeyNotFoundException()
    {
        // Arrange
        var command = GetProductByIdHandlerTestData.GenerateQuery();
        _productRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns((Product?)null);

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Product with ID {command.Id} not found");
        await _productRepository.Received(1).GetByIdAsync(command.Id, Arg.Any<CancellationToken>());
    }
}
