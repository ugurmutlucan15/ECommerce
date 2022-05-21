using CatalogService.Entities;

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CatalogService.Repositories.Interfaces
{
    public interface IProductRepository
    {
        IQueryable<Product> GetAll(Expression<Func<Product, bool>> expression = null);

        Task<Product> Get(Expression<Func<Product, bool>> expression = null);

        Task Insert(Product entity);

        Task<bool> Update(Product entity);

        Task<bool> Delete(int id);
    }
}