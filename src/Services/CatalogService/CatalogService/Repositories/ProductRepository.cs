using CatalogService.Data;
using CatalogService.Entities;
using CatalogService.Repositories.Interfaces;

using Microsoft.EntityFrameworkCore;

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CatalogService.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ECommerceContext _context;

        public ProductRepository(ECommerceContext context)
        {
            _context = context;
        }

        public IQueryable<Product> GetAll(Expression<Func<Product, bool>> expression = null)
        {
            return expression == null
                ? _context.Set<Product>().AsNoTracking()
                : _context.Set<Product>().AsNoTracking().Where(expression);
        }

        public async Task<Product> Get(Expression<Func<Product, bool>> expression = null)
        {
            return await _context.Set<Product>().FirstOrDefaultAsync(expression);
        }

        public async Task Insert(Product entity)
        {
            await _context.Set<Product>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> Update(Product entity)
        {
            _context.Set<Product>().Update(entity);
            return await _context.SaveChangesAsync() == 1;
        }

        public async Task<bool> Delete(int id)
        {
            var entity = await Get(m => m.Id == id);
            _context.Set<Product>().Remove(entity);
            return await _context.SaveChangesAsync() == 1;
        }
    }
}