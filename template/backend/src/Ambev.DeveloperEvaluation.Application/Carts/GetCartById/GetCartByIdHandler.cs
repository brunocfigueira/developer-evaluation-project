using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetCartById;

public class GetCartByIdHandler : IRequestHandler<GetCartByIdCommand, GetCartByIdResult?>
{
    private readonly IMapper _mapper;
    private readonly ICartRepository _cartRepository;

    public GetCartByIdHandler(IMapper mapper, ICartRepository cartRepository)
    {
        _mapper = mapper;
        _cartRepository = cartRepository;
    }

    public async Task<GetCartByIdResult> Handle(GetCartByIdCommand request, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetByIdAsync(request.Id, cancellationToken) ?? throw new ResourceNotFoundException($"Cart with id {request.Id} not found");
        return _mapper.Map<GetCartByIdResult>(cart);
    }
}
