using CartService.Entities;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace CartService.Repositories.Interfaces
{
    public interface ICartRepository
    {
        public Task<IEnumerable<Cart>> GetCarts(string userId);

        public Task<Cart> AddCart(Cart model);

        public Task<IEnumerable<Cart>> AddCarts(IEnumerable<Cart> model);

        public Task<bool> DeleteCart(string userId);
    }
}