using Ambev.DeveloperEvaluation.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class CartItem
    {
        public int Id { get; set; } 
        public int CartId { get; set; } 
        public Cart? Cart { get; set; } 
        public int ProductId { get; set; }
        public Product? Product { get; set; } 
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }

        public void ApplyDiscountRules()
        {
            if (Quantity < 4)
            {
                Discount = 0;
            }
            else if (Quantity >= 4 && Quantity < 10)
            {
                Discount = 0.10m;
            }
            else if (Quantity >= 10 && Quantity <= 20)
            {
                Discount = 0.20m;
            }
            else if (Quantity > 20)
            {
                throw new BusinessRuleException("It is not possible to sell more than 20 identical products.");
            }

            if(Product == null)
            {
                throw new InvalidOperationException("It was not possible to recover the price of the product.");
            }
            UnitPrice = Product.Price;
            Total = Quantity * Product.Price * (1 - Discount);
        }
    }
}
