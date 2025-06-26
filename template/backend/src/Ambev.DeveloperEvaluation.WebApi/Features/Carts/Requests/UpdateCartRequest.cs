using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.Requests;

public class UpdateCartRequest
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime Date { get; set; }    
    public IEnumerable<UpdateCartItemsRequest> Products { get; set; } = new List<UpdateCartItemsRequest>();
}

public class UpdateCartItemsRequest
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}
