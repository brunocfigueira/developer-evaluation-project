using Ambev.DeveloperEvaluation.Application.Carts.ChangeStatusCart;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class ChangeStatusCartHandlerTests
{
    private readonly ICartRepository _cartRepository = Substitute.For<ICartRepository>();
    private readonly ChangeStatusCartHandler _handler;

    public ChangeStatusCartHandlerTests()
    {
        _handler = new ChangeStatusCartHandler(_cartRepository);
    }

    [Fact]
    public async Task Handle_ValidStatus_UpdatesCartStatus()
    {
        var cart = new Cart { Id = 1, Status = CartStatus.Open, UserId = 1, User = new User(), Items = new List<CartItem>() };
        var command = new ChangeStatusCartCommand { Id = 1, Status = CartStatus.Completed };
        _cartRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns(cart);

        await _handler.Handle(command, CancellationToken.None);

        cart.Status.Should().Be(command.Status);
        await _cartRepository.Received(1).UpdateAsync(cart, Arg.Any<CancellationToken>());
    }

    [Theory]
    [InlineData(CartStatus.Completed)]
    [InlineData(CartStatus.Error)]
    [InlineData(CartStatus.Cancelled)]
    [InlineData(CartStatus.AwaitingPayment)]
    public async Task Handle_InvalidStatus_ThrowsBusinessRuleException(CartStatus status)
    {
        var cart = new Cart { Id = 2, Status = status, UserId = 1, User = new User(), Items = new List<CartItem>() };
        var command = new ChangeStatusCartCommand { Id = 2, Status = CartStatus.Open };
        _cartRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns(cart);

        var act = () => _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<BusinessRuleException>()
            .WithMessage($"The cart with id {cart.Id} cannot be changed. Current Status: {cart.Status}");
        await _cartRepository.DidNotReceive().UpdateAsync(cart, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_CartNotFound_ThrowsResourceNotFoundException()
    {
        var command = new ChangeStatusCartCommand { Id = 3, Status = CartStatus.Open };
        _cartRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns((Cart?)null);

        var act = () => _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<ResourceNotFoundException>()
            .WithMessage($"Cart with id {command.Id} not found");
        await _cartRepository.DidNotReceive().UpdateAsync(Arg.Any<Cart>(), Arg.Any<CancellationToken>());
    }
}
