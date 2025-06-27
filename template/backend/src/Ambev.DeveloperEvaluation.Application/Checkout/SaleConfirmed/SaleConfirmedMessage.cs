using Rebus.Handlers;

namespace Ambev.DeveloperEvaluation.Application.Checkout.SaleConfirmed
{
    public class SaleConfirmedMessage
    {
        public int OrderId { get; set; }                
        public DateTime SaleDate { get; set; } = DateTime.UtcNow;
    }

}
