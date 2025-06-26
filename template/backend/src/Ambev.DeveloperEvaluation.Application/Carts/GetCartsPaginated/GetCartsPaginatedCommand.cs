using Ambev.DeveloperEvaluation.Domain.Entities;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetCartsPaginated
{
    public class GetCartsPaginatedCommand : IRequest<(IEnumerable<GetCartsPaginatedResult> Items, int TotalCount)>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? Order { get; set; }
    }
}
