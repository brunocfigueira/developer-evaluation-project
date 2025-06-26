using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Carts.CreateCart
{
    public class CreateCartCommandValidator : AbstractValidator<CreateCartCommand>
    {

        public CreateCartCommandValidator(IProductRepository productRepository, IUserRepository userRepository)
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("Id is required")
                .GreaterThan(0)
                .WithMessage("Id must be greater than zero")
                .MustAsync(async (userId, cancellation) =>
                {
                    return await userRepository.ExistsAsync(userId, cancellation);
                })
                .WithMessage(x => $"User with id {x.UserId} does not exist");

            RuleFor(x => x.CreatedAt).NotEqual(default(DateTime))
                .WithMessage("Date is required");

            RuleFor(x => x.Items)
                .NotEmpty()
                .WithMessage("At least one item is required");

            RuleForEach(x => x.Items)
                .SetValidator(new CreateCartItemCommandValidator())
                .MustAsync(async (item, cancellation) =>
                {
                    return await productRepository.ExistsAsync(item.ProductId, cancellation);
                })
                .WithMessage((_, item) => $"Product with id {item.ProductId} does not exist");
        }
    }

    public class CreateCartItemCommandValidator : AbstractValidator<CreateCartItemsCommand>
    {
        public CreateCartItemCommandValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty()
                .WithMessage("Product id is required")
                .GreaterThan(0)
                .WithMessage("Product id must be greater than zero");
            RuleFor(x => x.Quantity)
                .NotEmpty()
                .WithMessage("Quantity is required")
                .GreaterThan(0)
                .WithMessage("Quantity must be greater than zero");
        }
    }
}
