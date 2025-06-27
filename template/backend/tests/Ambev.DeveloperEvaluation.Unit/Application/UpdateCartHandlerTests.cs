using Ambev.DeveloperEvaluation.Application.Carts.UpdateCart;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using Xunit;
using System.Collections.Generic;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class UpdateCartHandlerTests
{
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private readonly ICartRepository _cartRepository = Substitute.For<ICartRepository>();
    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
    private readonly IProductRepository _productRepository = Substitute.For<IProductRepository>();
    private readonly UpdateCartHandler _handler;

    public UpdateCartHandlerTests()
    {
        _handler = new UpdateCartHandler(_mapper, _cartRepository, _userRepository, _productRepository);
    }

  

    [Fact]
    public async Task Handle_InvalidRequest_ThrowsValidationException()
    {
        var command = new UpdateCartCommand { Id = 1, UserId = 1, Items = new List<UpdateCartItemsCommand>() };
        _userRepository.ExistsAsync(command.UserId, Arg.Any<CancellationToken>()).Returns(false);

        var act = () => _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<ValidationException>();
    }
}
