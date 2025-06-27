using Ambev.DeveloperEvaluation.Application.Sales.GetSalesPaginated;
using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.Mappings
{
    public class SaleMappingProfile : Profile
    {
        public SaleMappingProfile()
        {
            CreateMap<Sale, GetSalesPaginatedResult>();
            CreateMap<SaleItem, GetSaleItemsPaginatedResult>();
        }
    }
}
