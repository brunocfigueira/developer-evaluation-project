using Ambev.DeveloperEvaluation.Domain.Entities;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetCartById;

public class GetCartByIdCommand : IRequest<GetCartByIdResult>
{
    public int Id { get; set; }
}
