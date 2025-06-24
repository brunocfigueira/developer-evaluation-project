using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;

public class DeleteProductHandler : IRequestHandler<DeleteProductCommand>
{
    private readonly IProductRepository _productRepository;

    public DeleteProductHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var exists = await _productRepository.ExistsAsync(request.Id, cancellationToken);
        if (!exists)
            throw new ResourceNotFoundException($"Product with ID {request.Id} not found");

        await _productRepository.DeleteAsync(request.Id, cancellationToken);
    }
}