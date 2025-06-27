using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.ChangeStatusCart
{
    public class ChangeStatusCartHandler : IRequestHandler<ChangeStatusCartCommand>
    {
        private readonly ICartRepository _cartRepository;

        public ChangeStatusCartHandler(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task Handle(ChangeStatusCartCommand command, CancellationToken cancellationToken)
        {
            var cart = await _cartRepository.GetByIdAsync(command.Id, cancellationToken) ?? throw new ResourceNotFoundException($"Cart with id {command.Id} not found");
            ValidateCartAllowedStatus(cart);
            cart.Status = command.Status;
            cart.UpdatedAt = DateTime.UtcNow;
            await _cartRepository.UpdateAsync(cart, cancellationToken);
        }
        private void ValidateCartAllowedStatus(Cart cart)
        {
            if (cart.Status == CartStatus.Completed
                || cart.Status == CartStatus.Error
                || cart.Status == CartStatus.Cancelled
                || cart.Status == CartStatus.AwaitingPayment)
            {
                throw new BusinessRuleException($"The cart with id {cart.Id} cannot be changed. Current Status: {cart.Status.ToString()}");
            }
        }

    }
}
