using Ambev.DeveloperEvaluation.Application.Checkout.SaleConfirmed;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

public class SaleConfirmedHandlerTests
{
    private readonly ILogger<SaleConfirmedHandler> _logger = Substitute.For<ILogger<SaleConfirmedHandler>>();
    private readonly ISaleRepository _saleRepository = Substitute.For<ISaleRepository>();
    private readonly IOrderRepository _orderRepository = Substitute.For<IOrderRepository>();
    private readonly ICartRepository _cartRepository = Substitute.For<ICartRepository>();
    private readonly SaleConfirmedHandler _handler;

    public SaleConfirmedHandlerTests()
    {
        _handler = new SaleConfirmedHandler(_logger, _saleRepository, _orderRepository, _cartRepository);
    }

    [Fact]
    public async Task Handle_ValidMessage_CreatesSale()
    {
        var order = new Order { Id = 1, CartId = 2, CustomerName = "Test", TotalAmount = 100, BranchName = "A", Status = OrderStatus.Completed };
        var cart = new Cart { Id = 2, User = new User(), Items = new[] { new CartItem { ProductId = 1, Quantity = 1, UnitPrice = 10, Discount = 0, Total = 10 } } };
        var message = new SaleConfirmedMessage { OrderId = 1, SaleDate = DateTime.UtcNow };
        _orderRepository.GetByIdAsync(1).Returns(order);
        _cartRepository.GetByIdAsync(2).Returns(cart);
        _saleRepository.AddAsync(Arg.Any<Sale>()).Returns(new Sale());
        await _handler.Handle(message);
        await _saleRepository.Received(1).AddAsync(Arg.Any<Sale>());
    }

    [Fact]
    public async Task Handle_OrderNotFound_ThrowsResourceNotFoundException()
    {
        var message = new SaleConfirmedMessage { OrderId = 99, SaleDate = DateTime.UtcNow };
        _orderRepository.GetByIdAsync(99).Returns((Order?)null);
        await Assert.ThrowsAsync<ResourceNotFoundException>(() => _handler.Handle(message));
    }

    [Fact]
    public async Task Handle_CartNotFoundOrNoItems_LogsErrorAndCreatesSaleWithEmptyItems()
    {
        var order = new Order { Id = 1, CartId = 2, CustomerName = "Test", TotalAmount = 100, BranchName = "A", Status = OrderStatus.Completed };
        var message = new SaleConfirmedMessage { OrderId = 1, SaleDate = DateTime.UtcNow };
        _orderRepository.GetByIdAsync(1).Returns(order);
        _cartRepository.GetByIdAsync(2).Returns((Cart?)null);
        _saleRepository.AddAsync(Arg.Any<Sale>()).Returns(new Sale());
        await _handler.Handle(message);
        await _saleRepository.Received(1).AddAsync(Arg.Is<Sale>(s => !s.Items.Any()));
    }
}
