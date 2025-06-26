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
            cart.Status = command.Status;

            await _cartRepository.UpdateAsync(cart, cancellationToken);
        }

    }
}
