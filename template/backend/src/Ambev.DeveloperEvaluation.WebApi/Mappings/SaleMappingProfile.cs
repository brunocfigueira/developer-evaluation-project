using Ambev.DeveloperEvaluation.Application.Sales.GetSalesPaginated;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.Responses;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings;

public class SaleMappingProfile : Profile
{
    public SaleMappingProfile()
    {
        CreateMap<GetSalesPaginatedResult, SaleResponse>();          
        CreateMap<GetSaleItemsPaginatedResult, SaleItemsResponse>();
    }
}
