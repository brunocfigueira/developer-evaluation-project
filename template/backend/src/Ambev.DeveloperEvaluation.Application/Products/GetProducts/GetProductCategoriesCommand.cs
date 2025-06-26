using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProducts;

public class GetProductCategoriesCommand : IRequest<List<string>>
{
}
