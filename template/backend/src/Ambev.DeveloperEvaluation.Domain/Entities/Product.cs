namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class Product
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; }
    public string Image { get; set; } = string.Empty;   
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public Product()
    {
        CreatedAt = DateTime.UtcNow;      
    }
}