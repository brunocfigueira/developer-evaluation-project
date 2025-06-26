using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetCarts;

public class GetCartsByUserIdCommand : IRequest<IEnumerable<GetCartsByUserIdResult>>
{
    public int UserId { get; set; }
}
