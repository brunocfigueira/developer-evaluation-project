namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.Responses;

public class CartResponse
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime Date { get; set; }
    public IEnumerable<CartItemsResponse> Products { get; set; } = new List<CartItemsResponse>();
}

public class CartItemsResponse
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}
