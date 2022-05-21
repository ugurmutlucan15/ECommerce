using LoginService.Data;
using LoginService.Entities;
using LoginService.Repositories.Interfaces;

using Microsoft.EntityFrameworkCore;

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LoginService.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ECommerceContext _context;

        public UserRepository(ECommerceContext context)
        {
            _context = context;
        }

        public IQueryable<User> GetAll(Expression<Func<User, bool>> expression = null)
        {
            return expression == null
                ? _context.Set<User>().AsNoTracking()
                : _context.Set<User>().AsNoTracking().Where(expression);
        }

        public async Task<User> Get(Expression<Func<User, bool>> expression = null)
        {
            return await _context.Set<User>().FirstOrDefaultAsync(expression);
        }

        public async Task Insert(User entity)
        {
            await _context.Set<User>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> Update(User entity)
        {
            _context.Set<User>().Update(entity);
            return await _context.SaveChangesAsync() == 1;
        }

        public async Task<bool> Delete(int id)
        {
            var entity = await Get(m => m.Id == id);
            _context.Set<User>().Remove(entity);
            return await _context.SaveChangesAsync() == 1;
        }
    }
}