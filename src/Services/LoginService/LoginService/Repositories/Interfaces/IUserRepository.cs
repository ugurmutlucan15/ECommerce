using LoginService.Entities;

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LoginService.Repositories.Interfaces
{
    public interface IUserRepository
    {
        IQueryable<User> GetAll(Expression<Func<User, bool>> expression = null);

        Task<User> Get(Expression<Func<User, bool>> expression = null);

        Task Insert(User entity);

        Task<bool> Update(User entity);

        Task<bool> Delete(int id);
    }
}