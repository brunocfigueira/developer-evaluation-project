using Ambev.DeveloperEvaluation.Domain.Enums;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.UpdateCart;

public class UpdateCartCommand : IRequest<UpdateCartResult>
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime UpdatedAt { get; set; }    
    public ICollection<UpdateCartItemsCommand> Items { get; set; } = new List<UpdateCartItemsCommand>();
}

public class UpdateCartItemsCommand
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}
