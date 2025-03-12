using AutoMapper;
using Application.DTO;
using API.Entity;


namespace API.Mapping;

public class MappingOrder : Profile
{
    public MappingOrder()
    {
        CreateMap<Order, OrderResponse>()
        .ForMember(dest => dest.Order, opt => opt.MapFrom(src => src.ShopOrders))
            .AfterMap((src, dest) =>
            {
                dest.TotalAmount = src.TotalAmount;
                dest.DiscountAmount = src.DiscountAmount;
                dest.ApplyDiscount = src.ApplyDiscount;
            });
        CreateMap<Discount, DiscountResponse>();
        CreateMap<Shop, ShopResponse>();
        CreateMap<ShopOrder, ShopOrderResponse>()
            .ForMember(dest => dest.OrderItem, opt => opt.MapFrom(src => src.OrderItems));
        CreateMap<OrderItem, OrderItemResponse>()
            .ForMember(dest => dest.AddOn, opt => opt.MapFrom(src => src.AddOns));
        CreateMap<AddOn, AddOnResponse>();
    }
}