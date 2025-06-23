using Ambev.DeveloperEvaluation.Application.Users.CreateUser;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.CreateProduct;

public class CreateProductHandler : IRequestHandler<CreateProductCommand, int>
{
    private readonly IProductRepository _productRepository;

    public CreateProductHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var validator = new CreateProductValidator();
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        var product = new Product
        {
            Title = request.Title,
            Description = request.Description,
            Price = request.Price,
            Category = request.Category,
            Image = request.Image
        };

        await _productRepository.AddAsync(product, cancellationToken);
        return product.Id;
    }
}