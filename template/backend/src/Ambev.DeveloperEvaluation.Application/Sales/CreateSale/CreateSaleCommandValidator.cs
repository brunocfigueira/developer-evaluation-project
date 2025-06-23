using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

public class CreateSaleCommandValidator : AbstractValidator<CreateSaleCommand>
{
    public CreateSaleCommandValidator(IProductRepository productRepository)
    {
        RuleFor(x => x.SaleNumber).NotEmpty();
        RuleFor(x => x.SaleDate).NotEmpty();
        RuleFor(x => x.ClientId).NotEmpty();
        RuleFor(x => x.ClientName).NotEmpty();
        RuleFor(x => x.BranchId).NotEmpty();
        RuleFor(x => x.BranchName).NotEmpty();
        
        RuleFor(x => x.Items)
            .NotEmpty()
            .WithMessage("At least one item is required");
        
        RuleForEach(x => x.Items)
            .SetValidator(new CreateSaleItemDtoValidator())
            .MustAsync(async (item, cancellation) =>
            {
                var exists = await productRepository.ExistsAsync(item.ProductId, cancellation);
                return exists;
            })
            .WithMessage((_, item) => $"Product with id {item.ProductId} does not exist");
    }
}

public class CreateSaleItemDtoValidator : AbstractValidator<CreateSaleItemDto>
{
    public CreateSaleItemDtoValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.ProductName).NotEmpty();
        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .LessThanOrEqualTo(20)
            .WithMessage("Quantity must be between 1 and 20");
            
        RuleFor(x => x.UnitPrice).GreaterThan(0);
    }
}
