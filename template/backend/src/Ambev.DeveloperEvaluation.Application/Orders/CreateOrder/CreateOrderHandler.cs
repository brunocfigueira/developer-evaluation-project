using Ambev.DeveloperEvaluation.Application.Carts.CreateCart;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Orders.CreateOrder
{
    public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, CreateOrderResult>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;
        public CreateOrderHandler(IOrderRepository orderRepository, ICartRepository cartRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
            _mapper = mapper;
        }
        public async Task<CreateOrderResult> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
        {
            
            var cart = await _cartRepository.GetByIdAsync(command.CartId, cancellationToken) ?? throw new ResourceNotFoundException($"Cart with id {command.CartId} not found.");

            ValidateCartStatus(cart);

            var order = new Order
            {
                CartId = command.CartId,
                CustomerName = cart.User.Username,
                BranchName = command.BranchName,
                TotalAmount = cart.CalculateTotalAmount(),
                Status = command.Status,
            };

            var createdOrder = await _orderRepository.AddAsync(order, cancellationToken);
            return _mapper.Map<CreateOrderResult>(createdOrder);
        }

        private void ValidateCartStatus(Cart cart)
        {
            if (cart.Status != CartStatus.PendingCheckout)
            {
                throw new BusinessRuleException($"Cart with id {cart.Id} is not in a valid state for order creation. Current status: {cart.Status}");
            }
        }
    }
}
