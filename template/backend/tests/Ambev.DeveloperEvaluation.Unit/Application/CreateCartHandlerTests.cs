using Ambev.DeveloperEvaluation.Application.Carts.CreateCart;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using Xunit;
using System.Collections.Generic;
using System;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class CreateCartHandlerTests
{
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private readonly ICartRepository _cartRepository = Substitute.For<ICartRepository>();
    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
    private readonly IProductRepository _productRepository = Substitute.For<IProductRepository>();
    private readonly CreateCartHandler _handler;

    public CreateCartHandlerTests()
    {
        _handler = new CreateCartHandler(_mapper, _cartRepository, _userRepository, _productRepository);
    }

    [Fact]
    public async Task Handle_InvalidRequest_ThrowsValidationException()
    {
        var command = new CreateCartCommand { UserId = 2, Items = new List<CreateCartItemsCommand>() };
        _userRepository.ExistsAsync(command.UserId, Arg.Any<CancellationToken>()).Returns(false);

        var act = () => _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<ValidationException>();
    }
}
