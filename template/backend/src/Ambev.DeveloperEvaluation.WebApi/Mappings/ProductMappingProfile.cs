using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.Responses;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.Requests;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings;

public class ProductMappingProfile : Profile
{
    public ProductMappingProfile()
    {
        CreateMap<CreateProductRequest, CreateProductCommand>();
        CreateMap<CreateProductResult, ProductResponse>();
        CreateMap<ProductRatingResult, ProductRatingResponse>();
        CreateMap<UpdateProductRequest, UpdateProductCommand>();
        CreateMap<Product, ProductResponse>()
            .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => new ProductRatingResponse
            {
                Rate = 0, 
                Count = 0
            }));
    }
}