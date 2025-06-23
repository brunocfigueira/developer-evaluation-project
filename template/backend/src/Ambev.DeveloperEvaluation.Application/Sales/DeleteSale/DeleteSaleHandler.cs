using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;

public class DeleteSaleHandler : IRequestHandler<DeleteSaleCommand>
{
    private readonly ISaleRepository _repository;
    public DeleteSaleHandler(ISaleRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(DeleteSaleCommand request, CancellationToken cancellationToken)
    {
        await _repository.DeleteAsync(request.Id);
    }
}
