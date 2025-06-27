using Ambev.DeveloperEvaluation.Application.Carts.GetCartById;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class GetCartByIdHandlerTests
{
    private readonly ICartRepository _cartRepository = Substitute.For<ICartRepository>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private readonly GetCartByIdHandler _handler;

    public GetCartByIdHandlerTests()
    {
        _handler = new GetCartByIdHandler(_mapper, _cartRepository);
    }

    [Fact]
    public async Task Handle_CartExists_ReturnsMappedResult()
    {
        var cart = new Cart { Id = 1, UserId = 1, User = new User(), Items = new List<CartItem>() };
        var result = new GetCartByIdResult();
        var command = new GetCartByIdCommand { Id = 1 };
        _cartRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns(cart);
        _mapper.Map<GetCartByIdResult>(cart).Returns(result);

        var response = await _handler.Handle(command, CancellationToken.None);

        response.Should().Be(result);
        await _cartRepository.Received(1).GetByIdAsync(command.Id, Arg.Any<CancellationToken>());
        _mapper.Received(1).Map<GetCartByIdResult>(cart);
    }

    [Fact]
    public async Task Handle_CartNotFound_ThrowsResourceNotFoundException()
    {
        var command = new GetCartByIdCommand { Id = 2 };
        _cartRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns((Cart?)null);

        var act = () => _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<ResourceNotFoundException>()
            .WithMessage($"Cart with id {command.Id} not found");
    }
}
