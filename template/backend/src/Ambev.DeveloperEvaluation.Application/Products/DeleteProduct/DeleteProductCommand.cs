using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;

public class DeleteProductCommand : IRequest
{
    public int Id { get; set; }
}