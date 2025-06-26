using Ambev.DeveloperEvaluation.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Orders.CreateOrder
{
    public class CreateOrderCommand : IRequest<CreateOrderResult>
    {
        public int CartId { get; set; }
        public string BranchName { get; set; } = string.Empty;                
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
    }
}
