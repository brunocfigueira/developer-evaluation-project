using Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.DeleteCart
{
    public class DeleteCartHandler : IRequestHandler<DeleteCartCommand>
    {
        private readonly ICartRepository _cartRepository;

        public DeleteCartHandler(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }
        
        public async Task Handle(DeleteCartCommand command, CancellationToken cancellationToken)
        {
            var exists = await _cartRepository.ExistsAsync(command.Id, cancellationToken);
            if (!exists)
                throw new ResourceNotFoundException($"Cart with id {command.Id} not found");

            await _cartRepository.DeleteAsync(command.Id, cancellationToken);
        }
    }
}
