using AutoMapper;

using EventBusRabbitMQ.Events;

using OrderService.Entities;
using OrderService.Models;

namespace OrderService.Mapping
{
    public class Mappings : Profile
    {
        public Mappings()
        {
            #region Order

            CreateMap<Order, OrderSaveModel>()
                .IgnoreAllSourcePropertiesWithAnInaccessibleSetter()
                ;

            CreateMap<OrderSaveModel, Order>()
                .IgnoreAllSourcePropertiesWithAnInaccessibleSetter()
                ;

            CreateMap<Order, OrderViewModel>()
                .IgnoreAllSourcePropertiesWithAnInaccessibleSetter()
                ;

            #endregion Order

            CreateMap<CartEventModel, Order>()
                .IgnoreAllPropertiesWithAnInaccessibleSetter()
                .ForMember(m => m.Price, opt => opt.MapFrom(m => m.ProductPrice))
                ;
        }
    }
}