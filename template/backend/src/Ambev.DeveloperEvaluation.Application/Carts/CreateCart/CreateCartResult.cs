using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Application.Carts.CreateCart
{
    public class CreateCartResult
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public CartStatus Status { get; set; }       
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public IEnumerable<CreateCartItemsResult> Items { get; set; } = new List<CreateCartItemsResult>();
    }
    public class CreateCartItemsResult
    {
        public int Id { get; set; }      
        public int ProductId { get; set; }       
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }
    }
}