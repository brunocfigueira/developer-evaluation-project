using Ambev.DeveloperEvaluation.Application.Checkout.OrderConfirmed;
using Ambev.DeveloperEvaluation.Application.Checkout.PaymentFlow;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Rebus.Bus;
using Rebus.ServiceProvider;
using System.Threading.Tasks;
using Xunit;

public class OrderConfirmedHandlerTests
{
    private readonly IBusRegistry _busRegistry = Substitute.For<IBusRegistry>();
    private readonly IMediator _mediator = Substitute.For<IMediator>();
    private readonly ILogger<OrderConfirmedHandler> _logger = Substitute.For<ILogger<OrderConfirmedHandler>>();
    private readonly OrderConfirmedHandler _handler;

    public OrderConfirmedHandlerTests()
    {
        _handler = new OrderConfirmedHandler(_busRegistry, _mediator, _logger);
    }  
}
