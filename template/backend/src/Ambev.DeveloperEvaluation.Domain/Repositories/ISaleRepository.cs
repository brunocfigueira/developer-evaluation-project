using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

public interface ISaleRepository
{
    Task<Sale?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Sale> AddAsync(Sale Sale, CancellationToken cancellationToken = default);
    Task<Sale> UpdateAsync(Sale Sale, CancellationToken cancellationToken = default);
    Task<(IEnumerable<Sale> Sales, int TotalCount)> GetSalesPaginatedAsync(int page, int pageSize, string? order = null, CancellationToken cancellationToken = default);
}
