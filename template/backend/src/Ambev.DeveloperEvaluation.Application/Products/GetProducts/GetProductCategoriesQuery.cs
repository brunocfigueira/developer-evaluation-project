using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProducts;

public class GetProductCategoriesQuery : IRequest<List<string>>
{
}
