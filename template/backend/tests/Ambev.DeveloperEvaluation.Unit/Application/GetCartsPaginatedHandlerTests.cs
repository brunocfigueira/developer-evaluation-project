using Ambev.DeveloperEvaluation.Application.Carts.GetCartsPaginated;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;
using System.Collections.Generic;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class GetCartsPaginatedHandlerTests
{
    private readonly ICartRepository _cartRepository = Substitute.For<ICartRepository>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private readonly GetCartsPaginatedHandler _handler;

    public GetCartsPaginatedHandlerTests()
    {
        _handler = new GetCartsPaginatedHandler(_cartRepository, _mapper);
    }

    [Fact]
    public async Task Handle_ReturnsMappedResultsAndTotalCount()
    {
        var carts = new List<Cart> { new Cart { Id = 1, UserId = 1, User = new User(), Items = new List<CartItem>() } };
        var mapped = new List<GetCartsPaginatedResult> { new GetCartsPaginatedResult() };
        var command = new GetCartsPaginatedCommand { Page = 1, PageSize = 10, Order = null };
        _cartRepository.GetCartsPaginatedAsync(command.Page, command.PageSize, command.Order, Arg.Any<CancellationToken>()).Returns((carts, 1));
        _mapper.Map<IEnumerable<GetCartsPaginatedResult>>(carts).Returns(mapped);

        var (items, totalCount) = await _handler.Handle(command, CancellationToken.None);

        items.Should().BeEquivalentTo(mapped);
        totalCount.Should().Be(1);
        await _cartRepository.Received(1).GetCartsPaginatedAsync(command.Page, command.PageSize, command.Order, Arg.Any<CancellationToken>());
        _mapper.Received(1).Map<IEnumerable<GetCartsPaginatedResult>>(carts);
    }
}
