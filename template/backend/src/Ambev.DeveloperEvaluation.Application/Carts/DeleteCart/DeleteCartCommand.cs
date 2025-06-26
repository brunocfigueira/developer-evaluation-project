using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.DeleteCart
{
    public class DeleteCartCommand : IRequest
    {
        public int Id { get; set; }
    }
}
