using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Application.Carts.UpdateCart
{
    public class UpdateCartResult
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public CartStatus Status { get; set; }       
        public DateTime CreatedAt { get; set; } 
        public DateTime? UpdatedAt { get; set; }
        public ICollection<UpdateCartItemsResult> Items { get; set; } = new List<UpdateCartItemsResult>();
    }
    public class UpdateCartItemsResult
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }
    }
}
