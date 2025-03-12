using AutoMapper;

namespace Application.Mapping;

public class MappingOrder : Profile
{
    public MappingOrder()
    {
        CreateMap<DTO.Order, Domain.Entity.Order>().ReverseMap();
        CreateMap<DTO.Order, Domain.Entity.Order>()
            .ReverseMap()
            .AfterMap((src, dest) =>
            {
                dest.TotalAmount = src.GetTotalAmount();
                dest.DiscountAmount = src.GetDiscount();
                dest.ApplyDiscount = src.ApplyDiscount();
            });

        CreateMap<DTO.Discount, Domain.Entity.Discount>().ReverseMap();
        CreateMap<DTO.Shop, Domain.Entity.Shop>().ReverseMap();
        CreateMap<DTO.ShopOrder, Domain.Entity.ShopOrder>().ReverseMap();
        CreateMap<DTO.OrderItem, Domain.Entity.OrderItem>().ReverseMap();
        CreateMap<DTO.AddOn, Domain.Entity.AddOn>().ReverseMap();
    }
}