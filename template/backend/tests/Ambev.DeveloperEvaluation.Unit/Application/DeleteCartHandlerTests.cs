using Ambev.DeveloperEvaluation.Application.Carts.DeleteCart;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentAssertions;
using NSubstitute;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class DeleteCartHandlerTests
{
    private readonly ICartRepository _cartRepository = Substitute.For<ICartRepository>();
    private readonly DeleteCartHandler _handler;

    public DeleteCartHandlerTests()
    {
        _handler = new DeleteCartHandler(_cartRepository);
    }

    [Fact]
    public async Task Handle_ExistingCart_DeletesCart()
    {
        var command = new DeleteCartCommand { Id = 1 };
        _cartRepository.ExistsAsync(command.Id, Arg.Any<CancellationToken>()).Returns(true);

        await _handler.Handle(command, CancellationToken.None);

        await _cartRepository.Received(1).DeleteAsync(command.Id, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_NonExistingCart_ThrowsResourceNotFoundException()
    {
        var command = new DeleteCartCommand { Id = 2 };
        _cartRepository.ExistsAsync(command.Id, Arg.Any<CancellationToken>()).Returns(false);

        var act = () => _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<ResourceNotFoundException>()
            .WithMessage($"Cart with id {command.Id} not found");
        await _cartRepository.DidNotReceive().DeleteAsync(Arg.Any<int>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_DbUpdateException_ThrowsBusinessRuleException()
    {
        var command = new DeleteCartCommand { Id = 3 };
        _cartRepository.ExistsAsync(command.Id, Arg.Any<CancellationToken>()).Returns(true);
        _cartRepository.DeleteAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns<Task>(x => throw new DbUpdateException("fail", new Exception("inner")));

        var act = () => _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<BusinessRuleException>()
            .WithMessage($"Cart with id {command.Id} is currently in use and cannot be deleted.*");
    }
}
