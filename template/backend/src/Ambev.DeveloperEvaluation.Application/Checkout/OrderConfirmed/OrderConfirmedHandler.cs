using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Rebus.Bus;
using Rebus.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Checkout.OrderConfirmed
{
    public class OrderConfirmedHandler : IHandleMessages<OrderConfirmedMessage>
    {
        private readonly IBus _bus;
        private readonly ICartRepository _cartRepository;

        public OrderConfirmedHandler(IBus bus, ICartRepository cartRepository)
        {
            _bus = bus;
            _cartRepository = cartRepository;

        }
        public async Task Handle(OrderConfirmedMessage message)
        {
            Console.WriteLine($"Pedido confirmado: {message.OrderId} (Cart: {message.CartId}) em {message.CreatedAt}");

            var paymentOk = await SimulatePaymentProcessing(message.OrderId);

            if (!paymentOk)
            {
                Console.WriteLine($"Falha no processamento do pagamento para o pedido {message.OrderId}");
                return;
            }

            await ChangeCartStatusToCompleted(message.CartId);



            // Aqui você pode acionar pagamento, envio de e-mail, etc.
            await Task.Delay(500); // Simula processamento
        }
        private bool GenerateRandomBoolean()
        {
            Random random = new Random();
            return random.Next(2) == 1; // Retorna true ou false aleatoriamente
        }

        private async Task<bool> SimulatePaymentProcessing(Guid orderId)
        {
            // Simula o processamento de pagamento
            // Aqui você pode integrar com um serviço de pagamento real
            await Task.Delay(500);
            return GenerateRandomBoolean(); // Retorna true se o pagamento foi processado com sucesso
        }

        private async Task ChangeCartStatusToCompleted(int cartId)
        {
            var cart = await _cartRepository.GetByIdAsync(cartId);
            if(cart != null)
            {
                cart.Status = CartStatus.Completed;
                await _cartRepository.UpdateAsync(cart);
            }         
        }
    }
}
