using Ambev.DeveloperEvaluation.Domain.Entities;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProduct;

public class GetProductByIdCommand : IRequest<Product?>
{
    public int Id { get; set; }
}