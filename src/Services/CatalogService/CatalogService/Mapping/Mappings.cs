using AutoMapper;

using CatalogService.Entities;
using CatalogService.Models;

namespace CatalogService.Mapping
{
    public class Mappings : Profile
    {
        public Mappings()
        {
            #region Product

            CreateMap<Product, ProductSaveModel>()
                .IgnoreAllSourcePropertiesWithAnInaccessibleSetter()
                ;

            CreateMap<ProductSaveModel, Product>()
                .IgnoreAllSourcePropertiesWithAnInaccessibleSetter()
                ;

            CreateMap<Product, ProductViewModel>()
                .IgnoreAllSourcePropertiesWithAnInaccessibleSetter()
                ;

            #endregion Product
        }
    }
}