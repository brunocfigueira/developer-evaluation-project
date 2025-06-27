using Ambev.DeveloperEvaluation.Application.Checkout.PaymentFlow;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using Rebus.Bus;
using Rebus.Handlers;
using Rebus.ServiceProvider;
using Serilog;

namespace Ambev.DeveloperEvaluation.Application.Checkout.OrderConfirmed
{

    /// <summary>
    /// Aqui é onde inicia diversos fluxos de processamento após a confirmação do pedido.
    /// Vai depender de como a modelagem de negocio decide tratar cada fluxo
    /// Segue apenas um exemplo de como poderia ser implementado.
    /// </summary>
    public class OrderConfirmedHandler : IHandleMessages<OrderConfirmedMessage>
    {
        private readonly IBusRegistry _busRegistry;
        private readonly IMediator _mediator;
        private readonly ILogger<OrderConfirmedHandler> _logger;

        public OrderConfirmedHandler(IBusRegistry busRegistry, IMediator mediator, ILogger<OrderConfirmedHandler> logger)
        {
            _busRegistry = busRegistry;
            _mediator = mediator;
            _logger = logger;
        }
        public async Task Handle(OrderConfirmedMessage message)
        {
            _logger.LogInformation("Event received OrderConfirmed: OrderId={OrderId}, CartId={CartId}, CreatedAt={CreatedAt}",
               message.OrderId, message.CartId, message.CreatedAt);

            await StartPaymentFlow(message);

            await StartOrderBillingFlow(message);

            await StartProductStockUpdateFlow(message);

            await StartLogisticsAndShippingFlow(message);

            await StartCustomerNotificationFlow(message);
        }

        /// <summary>
        ///   Start Payment Flow
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private async Task StartPaymentFlow(OrderConfirmedMessage message)
        {
            _logger.LogInformation("Starting the payment flow for the order {OrderId}", message.OrderId);

            try
            {
                // Inicia o fluxo de pagamento
                var paymentCommand = new PaymentFlowCommand { CartId = message.CartId, OrderId = message.OrderId };
                await _mediator.Send(paymentCommand);
                _logger.LogInformation("Payment processed successfully for order {OrderId}", message.OrderId);                
            }
            catch (PaymentFailedException ex)
            {
                // TODO: Implementar lógica de tratamento de falha de pagamento
                // Aqui você pode enviar uma mensagem para uma fila de falhas ou registrar o erro em um log
                _logger.LogInformation(ex.Message);                
            }
        }
        private async Task StartOrderBillingFlow(OrderConfirmedMessage message)
        {            
            _logger.LogInformation("Starting Order Billing Flow");
            //TODO: Implementar lógica de faturamento do pedido (registrar a venda propriamente dita)
        }
        private async Task StartProductStockUpdateFlow(OrderConfirmedMessage message)
        {
            // TODO: Implementar lógica de atualização de estoque de produtos
            /**
             * Redução da quantidade do produto no estoque.
               - Verificação de itens esgotados.
               - Atualização em tempo real (especialmente em grandes promoções).
               - Gatilho para reabastecimento (quando atinge quantidade mínima).
             */
            _logger.LogInformation("Starting product stock update flow");
        }

        private async Task StartLogisticsAndShippingFlow(OrderConfirmedMessage message)
        {
            // TODO: Implementar lógica de logística e envio
            /**
             * Geração de ordem de separação (picking).
               - Embalagem dos produtos (packing).
               - Emissão de etiquetas de envio e nota fiscal.
               - Integração com transportadora (Correios, Jadlog, Loggi, etc.).
               - Atualização de status: Em Separação → Enviado → Em Trânsito → Entregue.
             */
            _logger.LogInformation("Starting Logistics and Shipping flow");
        }

        private async Task StartCustomerNotificationFlow(OrderConfirmedMessage message)
        {
            // TODO: Implementar lógica de notificação ao cliente
            /**
             * Envio de e-mail ou SMS de confirmação de pedido.
               - Notificação de envio com código de rastreio.
               - Atualizações sobre o status do pedido.
               - Solicitação de feedback após a entrega.
             */
            _logger.LogInformation("Starting Customer Notification Flow");
        }
    }
}
