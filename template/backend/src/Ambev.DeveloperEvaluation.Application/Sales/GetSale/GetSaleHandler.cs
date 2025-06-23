using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

public class GetSaleHandler : IRequestHandler<GetSaleQuery, Sale?>, IRequestHandler<GetAllSalesQuery, IEnumerable<Sale>>
{
    private readonly ISaleRepository _repository;
    public GetSaleHandler(ISaleRepository repository)
    {
        _repository = repository;
    }

    public async Task<Sale?> Handle(GetSaleQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetByIdAsync(request.Id);
    }

    public async Task<IEnumerable<Sale>> Handle(GetAllSalesQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetAllAsync();
    }
}
