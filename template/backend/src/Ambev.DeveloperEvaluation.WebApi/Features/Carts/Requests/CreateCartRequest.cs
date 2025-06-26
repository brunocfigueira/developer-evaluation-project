using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.Requests;

public class CreateCartRequest
{
    public int UserId { get; set; }    
    public DateTime Date { get; set; } 
    public IEnumerable<CreateCartItemsRequest> Products { get; set; } = new List<CreateCartItemsRequest>();
}

public class CreateCartItemsRequest
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}
