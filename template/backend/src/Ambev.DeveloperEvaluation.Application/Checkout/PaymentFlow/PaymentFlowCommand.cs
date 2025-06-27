using Ambev.DeveloperEvaluation.Domain.Enums;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Checkout.PaymentFlow
{
    public class PaymentFlowCommand : IRequest
    {
        public int OrderId { get; set; }
        public int CartId { get; set; }
    }

}
