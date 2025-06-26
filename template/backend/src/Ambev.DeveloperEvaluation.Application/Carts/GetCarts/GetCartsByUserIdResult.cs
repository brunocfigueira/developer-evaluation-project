using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetCarts
{

    public class GetCartsByUserIdResult
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public CartStatus Status { get; set; }      
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public ICollection<GetCartItemsByUserIdResult> Items { get; set; } = new List<GetCartItemsByUserIdResult>();
    }
    public class GetCartItemsByUserIdResult
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }
    }
}
