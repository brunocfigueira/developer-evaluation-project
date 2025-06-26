namespace Ambev.DeveloperEvaluation.WebApi.Features.Checkout.Requests
{
    public class CreateOrderRequest
    {
        public int CartId { get; set; }
        public string BranchName { get; set; } = string.Empty;
    }
}
