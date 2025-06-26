using Ambev.DeveloperEvaluation.Application.Carts.CreateCart;
using Ambev.DeveloperEvaluation.Application.Carts.GetCartById;
using Ambev.DeveloperEvaluation.Application.Carts.GetCarts;
using Ambev.DeveloperEvaluation.Application.Carts.GetCartsPaginated;
using Ambev.DeveloperEvaluation.Application.Carts.UpdateCart;
using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Carts.Mappings
{
    public class CartMappingProfile : Profile
    {
        public CartMappingProfile()
        {
            CreateMap<CreateCartCommand, Cart>()
                    .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                    .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                    .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

            CreateMap<CreateCartItemsCommand, CartItem>();

            CreateMap<UpdateCartCommand, Cart>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

            CreateMap<UpdateCartItemsCommand, CartItem>();

            CreateMap<Cart, CreateCartResult>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

            CreateMap<CartItem, CreateCartItemsResult>();

            CreateMap<Cart, UpdateCartResult>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

            CreateMap<CartItem, UpdateCartItemsResult>();

            CreateMap<Cart, GetCartByIdResult>();
            CreateMap<CartItem, GetCartItemsByIdResult>();

            CreateMap<Cart, GetCartsByUserIdResult>();
            CreateMap<CartItem, GetCartItemsByUserIdResult>();

            CreateMap<Cart, GetCartsPaginatedResult>();
            CreateMap<CartItem, GetCartItemsPaginatedResult>();
        }
    }
}
