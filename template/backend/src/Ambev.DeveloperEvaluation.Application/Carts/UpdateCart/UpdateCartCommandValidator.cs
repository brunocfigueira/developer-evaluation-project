using Ambev.DeveloperEvaluation.Application.Carts.CreateCart;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Carts.UpdateCart
{
    public class UpdateCartCommandValidator : AbstractValidator<UpdateCartCommand>
    {

        public UpdateCartCommandValidator(IProductRepository productRepository, IUserRepository userRepository)
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Id is required")
                .GreaterThan(0)
                .WithMessage("Id must be greater than zero");

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
           

            RuleFor(x => x.UpdatedAt).NotEqual(default(DateTime))
               .WithMessage("Date is required");

            RuleFor(x => x.Items)
                .NotEmpty()
                .WithMessage("At least one item is required");

            RuleForEach(x => x.Items)
                .SetValidator(new UpdateCartItemCommandValidator())
                .MustAsync(async (item, cancellation) =>
                {
                    return await productRepository.ExistsAsync(item.ProductId, cancellation);
                })
                .WithMessage((_, item) => $"Product with id {item.ProductId} does not exist");
        }
    }

    public class UpdateCartItemCommandValidator : AbstractValidator<UpdateCartItemsCommand>
    {
        public UpdateCartItemCommandValidator()
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
