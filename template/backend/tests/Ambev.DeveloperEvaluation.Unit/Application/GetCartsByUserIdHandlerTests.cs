using Ambev.DeveloperEvaluation.Application.Carts.GetCarts;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;
using FluentValidation;
using System.Collections.Generic;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class GetCartsByUserIdHandlerTests
{
    private readonly ICartRepository _cartRepository = Substitute.For<ICartRepository>();
    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private readonly GetCartsByUserIdHandler _handler;

    public GetCartsByUserIdHandlerTests()
    {
        _handler = new GetCartsByUserIdHandler(_cartRepository, _userRepository, _mapper);
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsMappedResults()
    {
        var carts = new List<Cart> { new Cart { Id = 1, UserId = 1, User = new User(), Items = new List<CartItem>() } };
        var mapped = new List<GetCartsByUserIdResult> { new GetCartsByUserIdResult() };
        var command = new GetCartsByUserIdCommand { UserId = 1 };
        _cartRepository.GetCartsByUserIdAsync(command.UserId, Arg.Any<CancellationToken>()).Returns(carts);
        _mapper.Map<IEnumerable<GetCartsByUserIdResult>>(carts).Returns(mapped);
        _userRepository.ExistsAsync(command.UserId, Arg.Any<CancellationToken>()).Returns(true);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().BeEquivalentTo(mapped);
        await _cartRepository.Received(1).GetCartsByUserIdAsync(command.UserId, Arg.Any<CancellationToken>());
        _mapper.Received(1).Map<IEnumerable<GetCartsByUserIdResult>>(carts);
    }

    [Fact]
    public async Task Handle_InvalidUser_ThrowsValidationException()
    {
        var command = new GetCartsByUserIdCommand { UserId = 2 };
        _userRepository.ExistsAsync(command.UserId, Arg.Any<CancellationToken>()).Returns(false);

        var act = () => _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<ValidationException>();
        await _cartRepository.DidNotReceive().GetCartsByUserIdAsync(command.UserId, Arg.Any<CancellationToken>());
    }
}
