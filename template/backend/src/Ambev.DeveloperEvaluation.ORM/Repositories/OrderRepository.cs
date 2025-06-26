using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.ORM.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DefaultContext _context;

        public OrderRepository(DefaultContext context)
        {
            _context = context;
        }

        public async Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Orders.Include(c => c.Cart)
                                        .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Order> AddAsync(Order Order, CancellationToken cancellationToken = default)
        {
            _context.Orders.Add(Order);
            await _context.SaveChangesAsync(cancellationToken);
            return Order;
        }

        public async Task<Order> UpdateAsync(Order Order, CancellationToken cancellationToken = default)
        {
            _context.Orders.Update(Order);
            await _context.SaveChangesAsync(cancellationToken);
            return Order;
        }

    }
}
