using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetCartsPaginated
{
    public class GetCartsPaginatedHandler : IRequestHandler<GetCartsPaginatedCommand, (IEnumerable<GetCartsPaginatedResult> Items, int TotalCount)>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;

        public GetCartsPaginatedHandler(ICartRepository cartRepository, IMapper mapper)
        {
            _cartRepository = cartRepository;
            _mapper = mapper;
        }

        public async Task<(IEnumerable<GetCartsPaginatedResult> Items, int TotalCount)> Handle(GetCartsPaginatedCommand command, CancellationToken cancellationToken)
        {
            var (items, totalCount) = await _cartRepository.GetCartsPaginatedAsync(command.Page, command.PageSize, command.Order, cancellationToken);
            return (_mapper.Map<IEnumerable<GetCartsPaginatedResult>>(items), totalCount);
        }
    }
}
