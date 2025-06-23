using Ambev.DeveloperEvaluation.Domain.Entities;
using MediatR;
using System.Collections.Generic;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProducts;

public class GetProductsByCategoryQuery : IRequest<(List<Product> Items, int TotalCount)>
{
    public string Category { get; set; } = string.Empty;
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? Order { get; set; }
}
