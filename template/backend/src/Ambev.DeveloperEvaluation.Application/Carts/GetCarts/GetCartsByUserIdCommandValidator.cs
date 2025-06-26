using Ambev.DeveloperEvaluation.Application.Carts.CreateCart;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetCarts
{
    public class GetCartsByUserIdCommandValidator : AbstractValidator<GetCartsByUserIdCommand>
    {
        public GetCartsByUserIdCommandValidator(IUserRepository userRepository)
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
        }

    }
}
