using Ambev.DeveloperEvaluation.Application.Carts.GetCartsPaginated;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSalesPaginated
{
    public class GetSalesPaginatedCommand : IRequest<(IEnumerable<GetSalesPaginatedResult> Items, int TotalCount)>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? Order { get; set; }
    }
}
