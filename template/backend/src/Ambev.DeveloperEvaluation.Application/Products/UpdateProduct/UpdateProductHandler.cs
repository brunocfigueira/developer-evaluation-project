using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;

public class UpdateProductHandler : IRequestHandler<UpdateProductCommand>
{
    private readonly IProductRepository _productRepository;

    public UpdateProductHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new ResourceNotFoundException($"Product with ID {request.Id} not found");

        product.Title = request.Title;
        product.Description = request.Description;
        product.Price = request.Price;
        product.Category = request.Category;
        product.Image = request.Image;
        product.UpdatedAt = DateTime.UtcNow;

        await _productRepository.UpdateAsync(product, cancellationToken);
    }
}