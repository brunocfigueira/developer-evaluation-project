using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;
using Ambev.DeveloperEvaluation.Application.Products.GetProduct;
using Ambev.DeveloperEvaluation.Application.Products.GetProducts;
using Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.Requests;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.Responses;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public ProductsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    /// <summary>
    /// Creates a new product
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponseWithData<ProductResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateProductRequest request, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<CreateProductCommand>(request);
        var productId = await _mediator.Send(command, cancellationToken);
        var query = new GetProductByIdQuery { Id = productId };
        var product = await _mediator.Send(query, cancellationToken);
        if (product == null)
            return NotFound(new { message = "Product not found after creation" });
        var response = _mapper.Map<ProductResponse>(product);
        return CreatedAtAction(nameof(GetById), new { id = productId }, response);
    }

    /// <summary>
    /// Gets a paginated list of products with ordering
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery(Name = "_page")] int page = 1,
        [FromQuery(Name = "_size")] int pageSize = 10,
        [FromQuery(Name = "_order")] string? order = null,
        CancellationToken cancellationToken = default)
    {
        var query = new GetProductsQuery { Page = page, PageSize = pageSize, Order = order };
        var (items, totalCount) = await _mediator.Send(query, cancellationToken);
        var products = _mapper.Map<List<ProductResponse>>(items);
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        var response = new
        {
            data = products,
            totalItems = totalCount,
            currentPage = page,
            totalPages
        };
        return Ok(response);
    }

    /// <summary>
    /// Gets a product by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var query = new GetProductByIdQuery { Id = id };
        var product = await _mediator.Send(query, cancellationToken);

        if (product == null)
            return NotFound(new { message = "Product not found" });

        var response = _mapper.Map<ProductResponse>(product);
        return Ok(response);
    }

    /// <summary>
    /// Updates a product
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateProductRequest request, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<UpdateProductCommand>(request);
        command.Id = id;
        var query = new GetProductByIdQuery { Id = id };
        var product = await _mediator.Send(query, cancellationToken);
        if (product == null)
            return NotFound(new { message = "Product not found" });

        await _mediator.Send(command, cancellationToken);

        var response = _mapper.Map<ProductResponse>(product);
        return Ok(response);
    }

    /// <summary>
    /// Deletes a product
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var query = new GetProductByIdQuery { Id = id };
        var product = await _mediator.Send(query, cancellationToken);
        if (product == null)
            return NotFound(new { message = "Product not found" });

        var command = new DeleteProductCommand { Id = id };
        await _mediator.Send(command, cancellationToken);

        return Ok(new { message = "Product deleted successfully" });
    }

    /// <summary>
    /// Gets all product categories
    /// </summary>
    [HttpGet("categories")]
    [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCategories(CancellationToken cancellationToken)
    {
        var categories = await _mediator.Send(new GetProductCategoriesQuery(), cancellationToken);
        return Ok(categories);
    }

    /// <summary>
    /// Gets products by category with pagination and ordering
    /// </summary>
    [HttpGet("category/{category}")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByCategory(
        [FromRoute] string category,
        [FromQuery(Name = "_page")] int page = 1,
        [FromQuery(Name = "_size")] int pageSize = 10,
        [FromQuery(Name = "_order")] string? order = null,
        CancellationToken cancellationToken = default)
    {
        var query = new GetProductsByCategoryQuery
        {
            Category = category,
            Page = page,
            PageSize = pageSize,
            Order = order
        };
        var (items, totalCount) = await _mediator.Send(query, cancellationToken);
        var products = _mapper.Map<List<ProductResponse>>(items);
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        var response = new
        {
            data = products,
            totalItems = totalCount,
            currentPage = page,
            totalPages
        };
        return Ok(response);
    }
}