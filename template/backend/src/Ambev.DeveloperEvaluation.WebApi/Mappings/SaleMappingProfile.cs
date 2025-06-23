using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings;

public class SaleMappingProfile : Profile
{
    public SaleMappingProfile()
    {
        CreateMap<CreateSaleRequest, CreateSaleCommand>();
        CreateMap<CreateSaleItemRequest, CreateSaleItemDto>();
        CreateMap<UpdateSaleRequest, UpdateSaleCommand>();
        CreateMap<CreateSaleRequest, UpdateSaleCommand>();
        CreateMap<CreateSaleItemRequest, UpdateSaleItemDto>();
        CreateMap<Sale, SaleResponse>();
        CreateMap<SaleItem, SaleItemResponse>();
    }
}
