using MediatR;
using Rebus.Handlers;

namespace Ambev.DeveloperEvaluation.Application.Checkout.OrderConfirmed
{
    public class OrderConfirmedMessage
    {
        public int OrderId { get; set; }
        public int CartId { get; set; }
        public string BranchName { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; }
    }
}
