using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, Guid>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IProductRepository _productRepository;
    private readonly CreateSaleCommandValidator _validator;

    public CreateSaleHandler(
        ISaleRepository saleRepository,
        IProductRepository productRepository)
    {
        _saleRepository = saleRepository;
        _productRepository = productRepository;
        _validator = new CreateSaleCommandValidator(productRepository);
    }

    public async Task<Guid> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        var sale = new Sale
        {
            SaleNumber = request.SaleNumber,
            SaleDate = request.SaleDate,
            ClientId = request.ClientId,
            ClientName = request.ClientName,
            BranchId = request.BranchId,
            BranchName = request.BranchName,
            Items = new List<SaleItem>()
        };

        foreach (var item in request.Items)
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId, cancellationToken)
                ?? throw new ValidationException($"Product with id {item.ProductId} does not exist");

            var saleItem = new SaleItem
            {
                ProductId = product.Id,
                ProductName = product.Title,
                Quantity = item.Quantity,
                UnitPrice = product.Price
            };
            sale.Items.Add(saleItem);
        }

        sale.CalculateTotalsAndDiscounts();
        await _saleRepository.AddAsync(sale);
        return sale.Id;
    }
}
