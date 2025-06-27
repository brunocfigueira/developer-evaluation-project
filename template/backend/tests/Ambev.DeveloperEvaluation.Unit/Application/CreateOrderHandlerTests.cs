using Ambev.DeveloperEvaluation.Application.Orders.CreateOrder;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;
using System.Threading;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class CreateOrderHandlerTests
{
    private readonly IOrderRepository _orderRepository = Substitute.For<IOrderRepository>();
    private readonly ICartRepository _cartRepository = Substitute.For<ICartRepository>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private readonly CreateOrderHandler _handler;

    public CreateOrderHandlerTests()
    {
        _handler = new CreateOrderHandler(_orderRepository, _cartRepository, _mapper);
    }

    [Fact]
    public async Task Handle_ValidRequest_CreatesOrderAndReturnsResult()
    {
        // Arrange
        var cart = new Cart
        {
            Id = 1,
            UserId = 2,
            Status = CartStatus.PendingCheckout,
            User = new User { Id = 2, Username = "testuser" },
            Items = new[] { new CartItem { ProductId = 1, Quantity = 2, Product = new Product { Id = 1, Price = 10 }, UnitPrice = 10, Total = 20 } }
        };
        var command = new CreateOrderCommand { CartId = 1, BranchName = "Branch A", Status = OrderStatus.Pending };
        var order = new Order
        {
            Id = 10,
            CartId = 1,
            CustomerName = cart.User.Username,
            BranchName = command.BranchName,
            TotalAmount = 20,
            Status = command.Status
        };
        var result = new CreateOrderResult { Id = 10, CartId = 1, BranchName = "Branch A", Status = OrderStatus.Pending };

        _cartRepository.GetByIdAsync(command.CartId, Arg.Any<CancellationToken>()).Returns(cart);
        _orderRepository.AddAsync(Arg.Any<Order>(), Arg.Any<CancellationToken>()).Returns(order);
        _mapper.Map<CreateOrderResult>(order).Returns(result);

        // Act
        var response = await _handler.Handle(command, CancellationToken.None);

        // Assert
        response.Should().Be(result);
        await _orderRepository.Received(1).AddAsync(Arg.Any<Order>(), Arg.Any<CancellationToken>());
        _mapper.Received(1).Map<CreateOrderResult>(order);
    }

    [Fact]
    public async Task Handle_CartNotFound_ThrowsResourceNotFoundException()
    {
        // Arrange
        var command = new CreateOrderCommand { CartId = 99, BranchName = "Branch B", Status = OrderStatus.Pending };
        _cartRepository.GetByIdAsync(command.CartId, Arg.Any<CancellationToken>()).Returns((Cart?)null);

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ResourceNotFoundException>()
            .WithMessage("Cart with id 99 not found.");
    }

    [Fact]
    public async Task Handle_CartWithInvalidStatus_ThrowsBusinessRuleException()
    {
        // Arrange
        var cart = new Cart
        {
            Id = 2,
            UserId = 3,
            Status = CartStatus.Completed,
            User = new User { Id = 3, Username = "otheruser" },
            Items = new[] { new CartItem { ProductId = 2, Quantity = 1, Product = new Product { Id = 2, Price = 5 }, UnitPrice = 5, Total = 5 } }
        };
        var command = new CreateOrderCommand { CartId = 2, BranchName = "Branch C", Status = OrderStatus.Pending };
        _cartRepository.GetByIdAsync(command.CartId, Arg.Any<CancellationToken>()).Returns(cart);

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BusinessRuleException>()
            .WithMessage($"Cart with id {cart.Id} is not in a valid state for order creation. Current status: {cart.Status}");
    }
}
