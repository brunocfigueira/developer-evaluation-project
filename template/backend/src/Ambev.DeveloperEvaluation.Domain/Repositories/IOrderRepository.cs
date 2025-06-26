using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories
{
    public interface IOrderRepository
    {
        Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);        
        Task<Order> AddAsync(Order order, CancellationToken cancellationToken = default);
        Task<Order> UpdateAsync(Order order, CancellationToken cancellationToken = default);        
    }
}
