using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

public class SaleRepository : ISaleRepository
{
    private readonly DefaultContext _context;
    public SaleRepository(DefaultContext context)
    {
        _context = context;
    }

    public async Task<Sale> AddAsync(Sale sale, CancellationToken cancellationToken)
    {

        _context.Sales.Add(sale);
        await _context.SaveChangesAsync(cancellationToken);
        return sale;

    }

    public async Task<Sale?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _context.Sales.Include(c => c.Items)
                                    .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<(IEnumerable<Sale> Sales, int TotalCount)> GetSalesPaginatedAsync(int page, int pageSize, string? order, CancellationToken cancellationToken)
    {
        var query = _context.Sales.Include(c => c.Items).AsNoTracking();
        if (!string.IsNullOrWhiteSpace(order))
        {
            foreach (var part in order.Split(','))
            {
                var trimmed = part.Trim();
                if (trimmed.StartsWith("saleNumber", StringComparison.OrdinalIgnoreCase))
                {
                    query = trimmed.EndsWith("desc", StringComparison.OrdinalIgnoreCase)
                        ? query.OrderByDescending(p => p.SaleNumber)
                        : query.OrderBy(p => p.SaleNumber);
                }
                else if (trimmed.StartsWith("saleDate", StringComparison.OrdinalIgnoreCase))
                {
                    query = trimmed.EndsWith("desc", StringComparison.OrdinalIgnoreCase)
                        ? query.OrderByDescending(p => p.SaleDate)
                        : query.OrderBy(p => p.SaleDate);
                }
                else if (trimmed.StartsWith("branchName", StringComparison.OrdinalIgnoreCase))
                {
                    query = trimmed.EndsWith("desc", StringComparison.OrdinalIgnoreCase)
                        ? query.OrderByDescending(p => p.BranchName)
                        : query.OrderBy(p => p.BranchName);
                }
                else if (trimmed.StartsWith("totalAmount", StringComparison.OrdinalIgnoreCase))
                {
                    query = trimmed.EndsWith("desc", StringComparison.OrdinalIgnoreCase)
                        ? query.OrderByDescending(p => p.TotalAmount)
                        : query.OrderBy(p => p.TotalAmount);
                }
                else if (trimmed.StartsWith("isCancelled", StringComparison.OrdinalIgnoreCase))
                {
                    query = trimmed.EndsWith("desc", StringComparison.OrdinalIgnoreCase)
                        ? query.OrderByDescending(p => p.IsCancelled)
                        : query.OrderBy(p => p.IsCancelled);
                }
            }
        }
        else
        {
            query = query.OrderBy(p => p.Id);
        }
        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
        return (items, totalCount);
    }

    public async Task<Sale> UpdateAsync(Sale sale, CancellationToken cancellationToken)
    {
        _context.Sales.Update(sale);
        await _context.SaveChangesAsync(cancellationToken);
        return sale;
    }
}
