using Ambev.DeveloperEvaluation.Application.Checkout.PaymentFlow;
using Ambev.DeveloperEvaluation.Application.Checkout.SaleConfirmed;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Rebus.ServiceProvider;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

public class PaymentFlowHandlerTests
{
    private readonly IBusRegistry _busRegistry = Substitute.For<IBusRegistry>();
    private readonly ICartRepository _cartRepository = Substitute.For<ICartRepository>();
    private readonly IOrderRepository _orderRepository = Substitute.For<IOrderRepository>();
    private readonly ILogger<PaymentFlowHandler> _logger = Substitute.For<ILogger<PaymentFlowHandler>>();
    private readonly PaymentFlowHandler _handler;

    public PaymentFlowHandlerTests()
    {
        _handler = new PaymentFlowHandler(_busRegistry, _cartRepository, _orderRepository, _logger);
    }
      

    [Fact]
    public async Task Handle_OrderNotFound_ThrowsResourceNotFoundException()
    {
        var command = new PaymentFlowCommand { CartId = 1, OrderId = 99 };
        _orderRepository.GetByIdAsync(99).Returns((Order?)null);
        await Assert.ThrowsAsync<ResourceNotFoundException>(() => _handler.Handle(command));
    }

    [Fact]
    public async Task Handle_CartNotFound_ThrowsResourceNotFoundException()
    {
        var command = new PaymentFlowCommand { CartId = 99, OrderId = 2 };
        var order = new Order { Id = 2, Status = OrderStatus.Pending };
        _orderRepository.GetByIdAsync(2).Returns(order);
        _cartRepository.GetByIdAsync(99).Returns((Cart?)null);
        await Assert.ThrowsAsync<ResourceNotFoundException>(() => _handler.Handle(command));
    }
}
