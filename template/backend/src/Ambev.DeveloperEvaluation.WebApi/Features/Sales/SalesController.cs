using Ambev.DeveloperEvaluation.Application.Sales.GetSalesPaginated;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.Responses;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales;

[ApiController]
[Route("api/[controller]")]
public class SalesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public SalesController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSaleByPaginated(
        [FromQuery(Name = "_page")] int page = 1,
        [FromQuery(Name = "_size")] int pageSize = 10,
        [FromQuery(Name = "_order")] string? order = null,
        CancellationToken cancellationToken = default)
    {
        var command = new GetSalesPaginatedCommand { Page = page, PageSize = pageSize, Order = order };
        var (items, totalCount) = await _mediator.Send(command, cancellationToken);
        var sales = _mapper.Map<IEnumerable<SaleResponse>>(items);
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        var response = new
        {
            data = sales,
            totalItems = totalCount,
            currentPage = page,
            totalPages
        };
        return Ok(response);
    }
}
