using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

public class CartRepository : ICartRepository
{
    private readonly DefaultContext _context;

    public CartRepository(DefaultContext context)
    {
        _context = context;
    }

    public async Task<Cart?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Carts.Include(c => c.Items).FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<Cart>> GetCartsByUserIdAsync(int userId, CancellationToken cancellationToken = default)
    {
        return await _context.Carts
            .Include(c => c.Items)
            .Where(c => c.UserId == userId)
            .ToListAsync(cancellationToken);
    }

    public async Task<Cart> AddAsync(Cart cart, CancellationToken cancellationToken = default)
    {
        _context.Carts.Add(cart);
        await _context.SaveChangesAsync(cancellationToken);
        return cart;
    }

    public async Task<Cart> UpdateAsync(Cart cart, CancellationToken cancellationToken = default)
    {
        _context.Carts.Update(cart);
        await _context.SaveChangesAsync(cancellationToken);
        return cart;
    }    
    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Carts.AnyAsync(p => p.Id == id, cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var cart = await _context.Carts.FindAsync(new object[] { id }, cancellationToken);
        if (cart != null)
        {
            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<(IEnumerable<Cart> Carts, int TotalCount)> GetCartsPaginatedAsync(int page, int pageSize, string? order = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Carts.Include(c => c.Items).AsNoTracking();
        if (!string.IsNullOrWhiteSpace(order))
        {
            foreach (var part in order.Split(','))
            {
                var trimmed = part.Trim();
                if (trimmed.StartsWith("id", StringComparison.OrdinalIgnoreCase))
                {
                    query = trimmed.EndsWith("desc", StringComparison.OrdinalIgnoreCase)
                        ? query.OrderByDescending(p => p.Id)
                        : query.OrderBy(p => p.Id);
                }
                else if (trimmed.StartsWith("userId", StringComparison.OrdinalIgnoreCase))
                {
                    query = trimmed.EndsWith("desc", StringComparison.OrdinalIgnoreCase)
                        ? query.OrderByDescending(p => p.UserId)
                        : query.OrderBy(p => p.UserId);
                }
                else if (trimmed.StartsWith("date", StringComparison.OrdinalIgnoreCase))
                {
                    query = trimmed.EndsWith("desc", StringComparison.OrdinalIgnoreCase)
                        ? query.OrderByDescending(p => (p.UpdatedAt != null)? p.UpdatedAt: p.CreatedAt)
                        : query.OrderBy(p => (p.UpdatedAt != null) ? p.UpdatedAt : p.CreatedAt);
                }
                else if (trimmed.StartsWith("status", StringComparison.OrdinalIgnoreCase))
                {
                    query = trimmed.EndsWith("desc", StringComparison.OrdinalIgnoreCase)
                        ? query.OrderByDescending(p =>  p.Status)
                        : query.OrderBy(p => p.Status);
                }
            }
        }
        else
        {
            query = query.OrderBy(p => p.UserId);
        }
        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
        return (items, totalCount);
    }
}
