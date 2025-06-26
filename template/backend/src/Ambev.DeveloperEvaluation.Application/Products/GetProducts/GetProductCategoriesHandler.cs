using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProducts;

public class GetProductCategoriesHandler : IRequestHandler<GetProductCategoriesCommand, List<string>>
{
    private readonly IProductRepository _productRepository;
    public GetProductCategoriesHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    public async Task<List<string>> Handle(GetProductCategoriesCommand request, CancellationToken cancellationToken)
    {
        return await _productRepository.GetCategoriesAsync(cancellationToken);
    }
}
