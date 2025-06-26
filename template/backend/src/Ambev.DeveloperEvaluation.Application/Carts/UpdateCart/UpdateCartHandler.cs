using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.UpdateCart;

public class UpdateCartHandler : IRequestHandler<UpdateCartCommand, UpdateCartResult>
{
    private readonly IMapper _mapper;
    private readonly ICartRepository _cartRepository;
    private readonly IUserRepository _userRepository;
    private readonly IProductRepository _productRepository;

    public UpdateCartHandler(IMapper mapper, ICartRepository cartRepository, IUserRepository userRepository, IProductRepository productRepository)
    {
        _mapper = mapper;
        _cartRepository = cartRepository;
        _userRepository = userRepository;
        _productRepository = productRepository;
    }

    public async Task<UpdateCartResult> Handle(UpdateCartCommand command, CancellationToken cancellationToken)
    {

        var validator = new UpdateCartCommandValidator(_productRepository, _userRepository);
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var cart = await _cartRepository.GetByIdAsync(command.Id) ?? throw new ResourceNotFoundException($"Cart with ID {command.Id} not found");
        cart.UserId = command.UserId;
        cart.UpdatedAt = command.UpdatedAt;

        UpdateCartItems(cart, command.Items);

        var updatedCart = await _cartRepository.UpdateAsync(cart, cancellationToken);

        return _mapper.Map<UpdateCartResult>(updatedCart);
    }

    private void UpdateCartItems(Cart cart, ICollection<UpdateCartItemsCommand> currenItems)
    {
        cart.Items.Clear();
        foreach (var current in currenItems)
        {
            var cartItem = _mapper.Map<CartItem>(current);
            cartItem.Product = _productRepository.GetByIdAsync(cartItem.ProductId).Result;
            cartItem.ApplyDiscountRules();
            cart.Items.Add(cartItem);
        }
    }
}
