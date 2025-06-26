using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;
using System.Collections.Generic;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProducts;

public class GetProductsByCategoryHandler : IRequestHandler<GetProductsByCategoryCommand, (List<Product> Items, int TotalCount)>
{
    private readonly IProductRepository _productRepository;
    public GetProductsByCategoryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    public async Task<(List<Product> Items, int TotalCount)> Handle(GetProductsByCategoryCommand request, CancellationToken cancellationToken)
    {
        return await _productRepository.GetByCategoryAsync(request.Category, request.Page, request.PageSize, request.Order, cancellationToken);
    }
}
