namespace Ambev.DeveloperEvaluation.Application.Sales.GetSalesPaginated
{
    public class GetSalesPaginatedResult
    {
        public int Id { get; set; }
        public string SaleNumber { get; set; } = string.Empty;
        public DateTime SaleDate { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public string BranchName { get; set; } = string.Empty;
        public bool IsCancelled { get; set; }
        public IEnumerable<GetSaleItemsPaginatedResult> Items { get; set; } = [];
    }
    public class GetSaleItemsPaginatedResult
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }
    }
}
