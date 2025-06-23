using Ambev.DeveloperEvaluation.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

public class GetSaleQuery : IRequest<Sale?>
{
    public Guid Id { get; set; }
}

public class GetAllSalesQuery : IRequest<IEnumerable<Sale>>
{
}
