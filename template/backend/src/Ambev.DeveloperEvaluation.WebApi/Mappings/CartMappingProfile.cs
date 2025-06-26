using Ambev.DeveloperEvaluation.Application.Carts.CreateCart;
using Ambev.DeveloperEvaluation.Application.Carts.GetCartById;
using Ambev.DeveloperEvaluation.Application.Carts.GetCarts;
using Ambev.DeveloperEvaluation.Application.Carts.GetCartsPaginated;
using Ambev.DeveloperEvaluation.Application.Carts.UpdateCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.Requests;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.Responses;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings;

public class CartMappingProfile : Profile
{
    public CartMappingProfile()
    {
    
        CreateMap<CreateCartRequest, CreateCartCommand>()
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.Date))
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Products));
        CreateMap<CreateCartItemsRequest, CreateCartItemsCommand>();

        CreateMap<UpdateCartRequest, UpdateCartCommand>()
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.Date))
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Products));
        CreateMap<UpdateCartItemsRequest, UpdateCartItemsCommand>();


        CreateMap<CreateCartResult, CartResponse>()
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.UpdatedAt != null ? src.UpdatedAt : src.CreatedAt))
            .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Items));
        CreateMap<CreateCartItemsResult, CartItemsResponse>();

        CreateMap<GetCartByIdResult, CartResponse>()
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.UpdatedAt != null ? src.UpdatedAt : src.CreatedAt))
            .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Items));
        CreateMap<GetCartItemsByIdResult, CartItemsResponse>();

        CreateMap<GetCartsByUserIdResult, CartResponse>()
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.UpdatedAt != null ? src.UpdatedAt : src.CreatedAt))
            .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Items));
        CreateMap<GetCartItemsByUserIdResult, CartItemsResponse>();

        CreateMap<UpdateCartResult, CartResponse>()
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.UpdatedAt != null ? src.UpdatedAt : src.CreatedAt))
            .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Items));
        CreateMap<UpdateCartItemsResult, CartItemsResponse>();

        CreateMap<GetCartsPaginatedResult, CartResponse>()
           .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.UpdatedAt != null ? src.UpdatedAt : src.CreatedAt))
           .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Items));
        CreateMap<GetCartItemsPaginatedResult, CartItemsResponse>();

    }
}
