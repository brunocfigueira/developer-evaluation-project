using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProduct;

public class GetProductByIdHandler : IRequestHandler<GetProductByIdCommand, Product?>
{
    private readonly IProductRepository _productRepository;

    public GetProductByIdHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Product?> Handle(GetProductByIdCommand request, CancellationToken cancellationToken)
    {
        return await _productRepository.GetByIdAsync(request.Id, cancellationToken) ?? throw new ResourceNotFoundException($"Product with ID {request.Id} not found");
    }
}