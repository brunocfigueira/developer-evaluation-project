using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

public interface ICartRepository
{
    Task<Cart?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Cart>> GetCartsByUserIdAsync(int userId, CancellationToken cancellationToken = default);
    Task<Cart> AddAsync(Cart cart, CancellationToken cancellationToken = default);
    Task<Cart> UpdateAsync(Cart cart, CancellationToken cancellationToken = default);    
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<(IEnumerable<Cart> Carts, int TotalCount)> GetCartsPaginatedAsync(int page, int pageSize, string? order = null, CancellationToken cancellationToken = default);
}
