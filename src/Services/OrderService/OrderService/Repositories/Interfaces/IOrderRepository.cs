using OrderService.Entities;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderService.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        public Task<IEnumerable<Order>> Gets(string userId);

        public Task<Order> Add(Order model);

        public Task<IEnumerable<Order>> Add(IEnumerable<Order> model);

        public Task<bool> Delete(string userId);
    }
}