using System;
using System.Collections.Generic;
using System.Linq;
using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class Sale : BaseEntity
{
    public string SaleNumber { get; set; } = string.Empty;
    public DateTime SaleDate { get; set; }
    public Guid ClientId { get; set; }
    public string ClientName { get; set; } = string.Empty;   
    public Guid BranchId { get; set; }
    public string BranchName { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public bool IsCancelled { get; set; }
    public List<SaleItem> Items { get; set; } = new();

    public void CalculateTotalsAndDiscounts()
    {
        foreach (var item in Items)
        {
            item.ApplyDiscountRules();
        }
        TotalAmount = Items.Sum(i => i.Total);
    }
}

public class SaleItem : BaseEntity
{   
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
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
            throw new InvalidOperationException("Não é possível vender mais de 20 itens idênticos.");
        }
        Total = Quantity * UnitPrice * (1 - Discount);
    }
}
