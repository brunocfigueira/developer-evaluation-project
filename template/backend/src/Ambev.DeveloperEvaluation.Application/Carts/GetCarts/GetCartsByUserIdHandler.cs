using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetCarts;

public class GetCartsByUserIdHandler : IRequestHandler<GetCartsByUserIdCommand, IEnumerable<GetCartsByUserIdResult>>
{
    private readonly ICartRepository _cartRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;


    public GetCartsByUserIdHandler(ICartRepository cartRepository, IUserRepository userRepository, IMapper mapper)
    {
        _cartRepository = cartRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GetCartsByUserIdResult>> Handle(GetCartsByUserIdCommand request, CancellationToken cancellationToken)
    {      
        var validator = new GetCartsByUserIdCommandValidator(_userRepository);
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var carts = await _cartRepository.GetCartsByUserIdAsync(request.UserId, cancellationToken);        
        return _mapper.Map<IEnumerable<GetCartsByUserIdResult>>(carts);
    }
}
