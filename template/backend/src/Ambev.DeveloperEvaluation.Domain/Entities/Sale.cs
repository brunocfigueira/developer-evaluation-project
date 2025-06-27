namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class Sale
{
    public int Id { get; set; }
    public string SaleNumber { get; set; } = DateTime.Now.ToString("yyyyMMddHHmmssfff");
    public string CustomerName { get; set; } = string.Empty;
    public DateTime SaleDate { get; set; }  
    public int OrderId { get; set; }
    public Order? Order { get; set; }
    public decimal TotalAmount { get; set; }
    public string BranchName { get; set; } = string.Empty;
    public bool IsCancelled { get; set; }
    public IEnumerable<SaleItem> Items { get; set; } = new List<SaleItem>();
}
