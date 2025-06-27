using Ambev.DeveloperEvaluation.Application.Carts.GetCartsPaginated;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSalesPaginated
{

    public class GetSalesPaginatedHandler : IRequestHandler<GetSalesPaginatedCommand, (IEnumerable<GetSalesPaginatedResult> Items, int TotalCount)>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;

        public GetSalesPaginatedHandler(ISaleRepository saleRepository, IMapper mapper)
        {
            _saleRepository = saleRepository;
            _mapper = mapper;
        }

        public async Task<(IEnumerable<GetSalesPaginatedResult> Items, int TotalCount)> Handle(GetSalesPaginatedCommand command, CancellationToken cancellationToken)
        {
            var (items, totalCount) = await _saleRepository.GetSalesPaginatedAsync(command.Page, command.PageSize, command.Order, cancellationToken);
            return (_mapper.Map<IEnumerable<GetSalesPaginatedResult>>(items), totalCount);
        }
    }
}
