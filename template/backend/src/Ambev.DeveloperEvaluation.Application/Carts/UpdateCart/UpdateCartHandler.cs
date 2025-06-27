using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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

        var cart = await _cartRepository.GetByIdAsync(command.Id) ?? throw new ResourceNotFoundException($"Cart with id {command.Id} not found");

        ValidateCartAllowedStatus(cart);

        cart.UserId = command.UserId;
        cart.UpdatedAt = command.UpdatedAt;

        UpdateCartItems(cart, command.Items);

        var updatedCart = await _cartRepository.UpdateAsync(cart, cancellationToken);

        return _mapper.Map<UpdateCartResult>(updatedCart);
    }
    private void ValidateCartAllowedStatus(Cart cart)
    {
        if(cart.Status == CartStatus.Completed 
            || cart.Status == CartStatus.Error 
            || cart.Status == CartStatus.Cancelled         
            || cart.Status == CartStatus.AwaitingPayment)
        {
            throw new BusinessRuleException($"The cart with id {cart.Id} cannot be changed. Current Status: {cart.Status.ToString()}");
        }
    }
    private void UpdateCartItems(Cart cart, IEnumerable<UpdateCartItemsCommand> currenItems)
    {
        
        var cartItemsList = cart.Items.ToList();
        cartItemsList.Clear(); 

        foreach (var current in currenItems)
        {
            var cartItem = _mapper.Map<CartItem>(current);
            cartItem.Product = _productRepository.GetByIdAsync(cartItem.ProductId).Result;
            cartItem.ApplyDiscountRules();
            cartItemsList.Add(cartItem);
        }

        cart.Items = cartItemsList; 
    }
}
