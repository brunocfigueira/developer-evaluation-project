using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.CreateCart;

public class CreateCartHandler : IRequestHandler<CreateCartCommand, CreateCartResult>
{
    private readonly IMapper _mapper;
    private readonly ICartRepository _cartRepository;
    private readonly IUserRepository _userRepository;
    private readonly IProductRepository _productRepository;

    public CreateCartHandler(IMapper mapper, ICartRepository cartRepository, IUserRepository userRepository, IProductRepository productRepository)
    {
        _mapper = mapper;
        _cartRepository = cartRepository;
        _userRepository = userRepository;
        _productRepository = productRepository;
    }

    public async Task<CreateCartResult> Handle(CreateCartCommand command, CancellationToken cancellationToken)
    {
        var validator = new CreateCartCommandValidator(_productRepository, _userRepository);
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var cart = _mapper.Map<Cart>(command);
     
        foreach (var item in cart.Items)
        {            
            item.Product = await _productRepository.GetByIdAsync(item.ProductId, cancellationToken);

            item.ApplyDiscountRules();
        }

        var createdCart = await _cartRepository.AddAsync(cart, cancellationToken);

        return _mapper.Map<CreateCartResult>(createdCart);
    }
}
