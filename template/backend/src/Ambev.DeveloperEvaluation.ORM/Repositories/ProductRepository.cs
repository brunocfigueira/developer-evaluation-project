using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly DefaultContext _context;

    public ProductRepository(DefaultContext context)
    {
        _context = context;
    }

    public async Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Products.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<(List<Product> Items, int TotalCount)> GetAllAsync(int page, int pageSize, string? order = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Products.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(order))
        {
            foreach (var part in order.Split(','))
            {
                var trimmed = part.Trim();
                if (trimmed.StartsWith("price", StringComparison.OrdinalIgnoreCase))
                {
                    query = trimmed.EndsWith("desc", StringComparison.OrdinalIgnoreCase)
                        ? query.OrderByDescending(p => p.Price)
                        : query.OrderBy(p => p.Price);
                }
                else if (trimmed.StartsWith("title", StringComparison.OrdinalIgnoreCase))
                {
                    query = trimmed.EndsWith("desc", StringComparison.OrdinalIgnoreCase)
                        ? query.OrderByDescending(p => p.Title)
                        : query.OrderBy(p => p.Title);
                }
            }
        }
        else
        {
            query = query.OrderBy(p => p.Title);
        }
        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
        return (items, totalCount);
    }

    public async Task<Product> AddAsync(Product product, CancellationToken cancellationToken = default)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync(cancellationToken);
        return product;
    }

    public async Task UpdateAsync(Product product, CancellationToken cancellationToken = default)
    {
        _context.Entry(product).State = EntityState.Modified;
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var product = await _context.Products.FindAsync(new object[] { id }, cancellationToken);
        if (product != null)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Products.AnyAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<List<string>> GetCategoriesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Products
            .Where(p => !string.IsNullOrEmpty(p.Category))
            .Select(p => p.Category)
            .Distinct()
            .OrderBy(c => c)
            .ToListAsync(cancellationToken);
    }

    public async Task<(List<Product> Items, int TotalCount)> GetByCategoryAsync(string category, int page, int pageSize, string? order, CancellationToken cancellationToken = default)
    {
        var query = _context.Products.AsNoTracking().Where(p => p.Category == category);
        if (!string.IsNullOrWhiteSpace(order))
        {
            foreach (var part in order.Split(','))
            {
                var trimmed = part.Trim();
                if (trimmed.StartsWith("price", StringComparison.OrdinalIgnoreCase))
                {
                    query = trimmed.EndsWith("desc", StringComparison.OrdinalIgnoreCase)
                        ? query.OrderByDescending(p => p.Price)
                        : query.OrderBy(p => p.Price);
                }
                else if (trimmed.StartsWith("title", StringComparison.OrdinalIgnoreCase))
                {
                    query = trimmed.EndsWith("desc", StringComparison.OrdinalIgnoreCase)
                        ? query.OrderByDescending(p => p.Title)
                        : query.OrderBy(p => p.Title);
                }
            }
        }
        else
        {
            query = query.OrderBy(p => p.Title);
        }
        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
        return (items, totalCount);
    }
}