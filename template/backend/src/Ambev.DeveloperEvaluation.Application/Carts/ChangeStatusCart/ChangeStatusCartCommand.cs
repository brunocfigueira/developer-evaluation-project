using Ambev.DeveloperEvaluation.Domain.Enums;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.ChangeStatusCart
{
    public class ChangeStatusCartCommand : IRequest
    {
        public int Id { get; set; }
        public CartStatus Status { get; set; }
    }
}
