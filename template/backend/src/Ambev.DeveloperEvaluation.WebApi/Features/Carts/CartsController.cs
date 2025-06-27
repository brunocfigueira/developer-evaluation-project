using Ambev.DeveloperEvaluation.Application.Carts.ChangeStatusCart;
using Ambev.DeveloperEvaluation.Application.Carts.CreateCart;
using Ambev.DeveloperEvaluation.Application.Carts.DeleteCart;
using Ambev.DeveloperEvaluation.Application.Carts.GetCartById;
using Ambev.DeveloperEvaluation.Application.Carts.GetCarts;
using Ambev.DeveloperEvaluation.Application.Carts.GetCartsPaginated;
using Ambev.DeveloperEvaluation.Application.Carts.UpdateCart;
using Ambev.DeveloperEvaluation.Application.Products.GetProducts;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.Requests;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.Responses;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.Responses;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts;

[ApiController]
[Route("api/[controller]")]
public class CartsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public CartsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }
    [HttpPost]
    [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateCart([FromBody] CreateCartRequest request, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<CreateCartCommand>(request);
        var resultCart = await _mediator.Send(command, cancellationToken);
        var response = _mapper.Map<CartResponse>(resultCart);

        return CreatedAtAction(nameof(GetCartById), new { id = response.Id }, response);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetCartById(int id, CancellationToken cancellationToken)
    {
        var resultCart = await _mediator.Send(new GetCartByIdCommand { Id = id }, cancellationToken);
        var response = _mapper.Map<CartResponse>(resultCart);
        return Ok(response);
    }

    [HttpGet("UserId/{userId}")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetCartsByUserId(int userId, CancellationToken cancellationToken)
    {
        var resultCarts = await _mediator.Send(new GetCartsByUserIdCommand { UserId = userId }, cancellationToken);
        var response = _mapper.Map<IEnumerable<CartResponse>>(resultCarts);
        return Ok(response);
    }


    [HttpPut("{id}")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateCart(int id, [FromBody] UpdateCartRequest request, CancellationToken cancellationToken)
    {
        request.Id = id;
        var command = _mapper.Map<UpdateCartCommand>(request);
        var resultCart = await _mediator.Send(command, cancellationToken);
        var response = _mapper.Map<CartResponse>(resultCart);
        return Ok(response);
    }

    [HttpPatch("{id}/StartCheckout")]    
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ChangeStatusToCheckout(int id, CancellationToken cancellationToken)
    {
        var command = new ChangeStatusCartCommand { Id = id, Status = CartStatus.PendingCheckout };
        await _mediator.Send(command, cancellationToken);
        return Ok(new { message = "Cart is at checkout" });
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCart(int id, CancellationToken cancellationToken)
    {
        var command = new DeleteCartCommand { Id = id };
        await _mediator.Send(command, cancellationToken);

        return Ok(new { message = "Cart deleted successfully" });
    }

    [HttpGet]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCartsByPaginated(
        [FromQuery(Name = "_page")] int page = 1,
        [FromQuery(Name = "_size")] int pageSize = 10,
        [FromQuery(Name = "_order")] string? order = null,
        CancellationToken cancellationToken = default)
    {
        var command = new GetCartsPaginatedCommand { Page = page, PageSize = pageSize, Order = order };
        var (items, totalCount) = await _mediator.Send(command, cancellationToken);
        var carts = _mapper.Map<IEnumerable<CartResponse>>(items);
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        var response = new
        {
            data = carts,
            totalItems = totalCount,
            currentPage = page,
            totalPages
        };
        return Ok(response);
    }
}
