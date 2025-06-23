using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;

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

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSaleRequest request, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<CreateSaleCommand>(request);
        var saleId = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = saleId }, new ApiResponseWithData<Guid> { Success = true, Data = saleId });
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var sales = await _mediator.Send(new GetAllSalesQuery(), cancellationToken);
        var response = _mapper.Map<List<SaleResponse>>(sales);
        return Ok(new ApiResponseWithData<List<SaleResponse>> { Success = true, Data = response });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var sale = await _mediator.Send(new GetSaleQuery { Id = id }, cancellationToken);
        if (sale == null)
            return NotFound(new ApiResponse { Success = false, Message = "Sale not found" });
        var response = _mapper.Map<SaleResponse>(sale);
        return Ok(new ApiResponseWithData<SaleResponse> { Success = true, Data = response });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateSaleRequest request, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<UpdateSaleCommand>(request);
        command.Id = id;
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteSaleCommand { Id = id }, cancellationToken);
        return NoContent();
    }
}
