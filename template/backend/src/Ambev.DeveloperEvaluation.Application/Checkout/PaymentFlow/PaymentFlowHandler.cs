using Ambev.DeveloperEvaluation.Application.Checkout.SaleConfirmed;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using Rebus.ServiceProvider;

namespace Ambev.DeveloperEvaluation.Application.Checkout.PaymentFlow
{
    /// <summary>
    ///  Aqui é onde o fluxo de pagamento é iniciado.
    ///  Vai depender de como a modelagem de negocio decide tratar o fluxo de pagamento.
    ///  Segue apenas um exemplo de como poderia ser implementado.
    /// </summary>
    public class PaymentFlowHandler : IRequestHandler<PaymentFlowCommand>
    {
        private readonly IBusRegistry _busRegistry;
        private readonly ICartRepository _cartRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<PaymentFlowHandler> _logger;

        public PaymentFlowHandler(IBusRegistry busRegistry, ICartRepository cartRepository, IOrderRepository orderRepository, ILogger<PaymentFlowHandler> logger)
        {
            _busRegistry = busRegistry;
            _cartRepository = cartRepository;
            _orderRepository = orderRepository;
            _logger = logger;
        }

        public async Task Handle(PaymentFlowCommand command, CancellationToken cancellationToken = default)
        {
            // TODO: Implementar recuperacao de forma de pagamento e demais informações necessárias para o processamento do pagamento

            var paymentStatus = SimulatePaymentCall().GetAwaiter().GetResult();
            await UpdateCartAndOrderStatus(command, paymentStatus);

            await SendSaleConfirmedMessage(command);
        }

        private async Task SendSaleConfirmedMessage(PaymentFlowCommand command)
        {
            _logger.LogInformation("Starting the billing flow for the order {OrderId}", command.OrderId);

            var message = new SaleConfirmedMessage { OrderId = command.OrderId };
            var saleBus = _busRegistry.GetBus("sale-confirmation");
            await saleBus.Send(message);
        }
        private async Task UpdateCartAndOrderStatus(PaymentFlowCommand command, PaymentStatus paymentStatus)
        {
            if (paymentStatus == PaymentStatus.Failed)
            {
                // TODO: Implementar lógica de tratamento de falha de pagamento
                /*
                 * Exemplo de envio de mensagem de falha de pagamento
                _bus.Send(new PaymentMessage
                {
                    OrderId = command.OrderId,
                    CartId = command.CartId,
                    CreatedAt = DateTime.UtcNow
                });
                */
                // throw new PaymentFailedException($"Payment processing failed for order {command.OrderId}. PaymentStatus: {paymentStatus.ToString()}");
                return;
            }

            if (paymentStatus == PaymentStatus.Pending)
            {
                await UpdateOrderStatus(command.OrderId, OrderStatus.AwaitingPayment);
                await UpdateCartStatus(command.CartId, CartStatus.AwaitingPayment);
                // TODO: Implementar lógica de tratamento de pagamento pendente
                /*
                 * Exemplo de envio de mensagem de pagamento pendente
                _bus.Send(new PaymentMessage
                {
                    OrderId = command.OrderId,
                    CartId = command.CartId,
                    CreatedAt = DateTime.UtcNow
                });
                */
                //throw new PaymentFailedException($"Payment processing failed for order {command.OrderId}. PaymentStatus: {paymentStatus.ToString()}");
                return;
            }
            if (paymentStatus == PaymentStatus.Refunded || paymentStatus == PaymentStatus.Cancelled)
            {
                await UpdateOrderStatus(command.OrderId, OrderStatus.Refunded);
                await UpdateCartStatus(command.CartId, CartStatus.Error);
                // TODO: Implementar lógica de tratamento de pagamento cesuado ou cancelamento ou reembolso
                /*
                 * Exemplo de envio de mensagem de pagamento pendente
                _bus.Send(new PaymentMessage
                {
                    OrderId = command.OrderId,
                    CartId = command.CartId,
                    CreatedAt = DateTime.UtcNow
                });
                */
                //throw new PaymentFailedException($"Payment processing failed for order {command.OrderId}. PaymentStatus: {paymentStatus.ToString()}");
                return;
            }

            await UpdateOrderStatus(command.OrderId, OrderStatus.Completed);
            await UpdateCartStatus(command.CartId, CartStatus.Completed);
        }

        private async Task UpdateOrderStatus(int orderId, OrderStatus status)
        {
            var order = await _orderRepository.GetByIdAsync(orderId) ?? throw new ResourceNotFoundException($"Order with id {orderId} not found.");
            order.Status = status;
            order.UpdatedAt = DateTime.UtcNow;
            await _orderRepository.UpdateAsync(order);
        }

        private async Task UpdateCartStatus(int cartId, CartStatus status)
        {
            var cart = await _cartRepository.GetByIdAsync(cartId) ?? throw new ResourceNotFoundException($"Cart with idD {cartId} not found.");
            cart.Status = status;
            cart.UpdatedAt = DateTime.UtcNow;
            await _cartRepository.UpdateAsync(cart);
        }

        private async Task<PaymentStatus> SimulatePaymentCall()
        {
            // Lista dos possíveis status de retorno da API de pagamento
            PaymentStatus[] possibleStatuses = new[]
            {
                // coloquei apenas 2 status para simplificar o exemplo
                PaymentStatus.Completed,
                PaymentStatus.Cancelled
            };

            // Seleciona um status aleatório
            var random = Random.Shared;
            PaymentStatus selectedStatus = possibleStatuses[random.Next(possibleStatuses.Length)];
            await Task.Delay(3000);


            return selectedStatus;
        }
    }
}
