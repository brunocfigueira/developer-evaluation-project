using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

public class UpdateSaleHandler : IRequestHandler<UpdateSaleCommand>
{
    private readonly ISaleRepository _repository;
    public UpdateSaleHandler(ISaleRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(UpdateSaleCommand request, CancellationToken cancellationToken)
    {
        var sale = await _repository.GetByIdAsync(request.Id);
        if (sale == null) throw new Exception("Sale not found");
        sale.SaleNumber = request.SaleNumber;
        sale.SaleDate = request.SaleDate;
        sale.ClientId = request.ClientId;
        sale.ClientName = request.ClientName;
        sale.BranchId = request.BranchId;
        sale.BranchName = request.BranchName;
        sale.IsCancelled = request.IsCancelled;
        sale.Items = request.Items.Select(i => new SaleItem
        {
            Id = i.Id == Guid.Empty ? Guid.NewGuid() : i.Id,
            ProductId = i.ProductId,
            ProductName = i.ProductName,
            Quantity = i.Quantity,
            UnitPrice = i.UnitPrice
        }).ToList();
        sale.CalculateTotalsAndDiscounts();
        await _repository.UpdateAsync(sale);
    }
}
