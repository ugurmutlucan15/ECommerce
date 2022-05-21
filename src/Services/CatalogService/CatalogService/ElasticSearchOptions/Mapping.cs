using CatalogService.Entities;

using Nest;

namespace CatalogService.ElasticSearchOptions
{
    public static class Mapping
    {
        public static CreateIndexDescriptor ProductMapping(this CreateIndexDescriptor descriptor)
        {
            return descriptor.Map<Product>(m => m.Properties(p => p
                .Keyword(k => k.Name(n => n.Id))
                .Text(t => t.Name(n => n.Name))
                .Number(t => t.Name(n => n.Price))
            )
            );
        }
    }
}