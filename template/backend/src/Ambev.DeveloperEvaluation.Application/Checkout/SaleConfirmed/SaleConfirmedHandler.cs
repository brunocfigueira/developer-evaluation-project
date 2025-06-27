using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.Extensions.Logging;
using Rebus.Bus;
using Rebus.Handlers;
using System.Collections.Generic;

namespace Ambev.DeveloperEvaluation.Application.Checkout.SaleConfirmed
{
    public class SaleConfirmedHandler : IHandleMessages<SaleConfirmedMessage>
    {        
        private readonly ILogger<SaleConfirmedHandler> _logger;
        private readonly ISaleRepository _saleRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly ICartRepository _cartRepository;


        public SaleConfirmedHandler(ILogger<SaleConfirmedHandler> logger, ISaleRepository saleRepository, IOrderRepository orderRepository, ICartRepository cartRepository)
        {            
            _logger = logger;
            _saleRepository = saleRepository;
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
        }
        public async Task Handle(SaleConfirmedMessage message)
        {
            _logger.LogInformation("Event received Sale Confirmed for the orderId {OrderId}", message.OrderId);

            var order = await RetrieveOrderAsync(message.OrderId); 
            var sale = new Sale
            {                
                OrderId = order.Id,
                CustomerName = order.CustomerName,
                TotalAmount = order.TotalAmount,
                BranchName = order.BranchName,
                SaleDate = message.SaleDate,
                IsCancelled = order.Status == OrderStatus.Cancelled,
                Items = await getOrderItemsAsync(order)
            };

            await _saleRepository.AddAsync(sale);

            _logger.LogInformation("Sale created successfully.");
            
        }

        private async Task<List<SaleItem>> getOrderItemsAsync(Order order)
        {
            var cart = await _cartRepository.GetByIdAsync(order.CartId);
            if (cart == null || cart.Items == null)
            {
                _logger.LogError("Cart with id {CartId} not found or has no items", order.CartId);
               return new List<SaleItem>();
            }

            return cart.Items.Select(item => new SaleItem
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                Discount = item.Discount,
                Total = item.Total
            }).ToList();
        }

        private async Task<Order> RetrieveOrderAsync(int orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
            {
                _logger.LogError("Order with id {OrderId} not found", orderId);
                throw new ResourceNotFoundException($"Order with id {orderId} not found");
            }
            return order;
        }
    }
}

