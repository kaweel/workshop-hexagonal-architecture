using AutoMapper;

namespace Infrastructure.Mapping;

public class MappingOrderEntityToDto : Profile
{
    public MappingOrderEntityToDto()
    {
        CreateMap<Mssql.Entity.Order, Application.DTO.Order>().ReverseMap();
        CreateMap<Mssql.Entity.Discount, Application.DTO.Discount>().ReverseMap();
        CreateMap<Mssql.Entity.Shop, Application.DTO.Shop>().ReverseMap();
        CreateMap<Mssql.Entity.ShopOrder, Application.DTO.ShopOrder>().ReverseMap();
        CreateMap<Mssql.Entity.OrderItem, Application.DTO.OrderItem>().ReverseMap();
        CreateMap<Mssql.Entity.AddOn, Application.DTO.AddOn>().ReverseMap();
    }
}
