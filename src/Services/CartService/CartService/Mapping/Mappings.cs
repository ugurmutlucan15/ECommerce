using AutoMapper;

using CartService.Entities;
using CartService.Extensions;
using CartService.Models;

namespace CartService.Mapping
{
    public class Mappings : Profile
    {
        public Mappings()
        {
            #region Cart

            CreateMap<Cart, CartSaveModel>()
                .IgnoreAllSourcePropertiesWithAnInaccessibleSetter()
                ;

            CreateMap<CartSaveModel, Cart>()
                .IgnoreAllSourcePropertiesWithAnInaccessibleSetter()
                .Ignore(m => m.UserId)
                ;

            CreateMap<Cart, CartViewModel>()
                .IgnoreAllSourcePropertiesWithAnInaccessibleSetter()
                ;

            #endregion Cart
        }
    }
}