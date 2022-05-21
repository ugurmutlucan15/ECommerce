using Microsoft.Extensions.Caching.Distributed;

using Newtonsoft.Json;

using OrderService.Entities;
using OrderService.Repositories.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IDistributedCache _redisCache;

        public OrderRepository(IDistributedCache redisCache)
        {
            _redisCache = redisCache;
        }

        public async Task<IEnumerable<Order>> Gets(string userId)
        {
            var model = await _redisCache.GetStringAsync("Orders." + userId);
            if (model == null)
            {
                return new List<Order>();
            }
            return JsonConvert.DeserializeObject<List<Order>>(model);
        }

        public async Task<Order> Add(Order model)
        {
            var res = await _redisCache.GetStringAsync("Orders." + model.UserId.ToString());
            if (string.IsNullOrEmpty(res))
            {
                var list = new List<Order> { model };
                await _redisCache.SetStringAsync("Orders." + model.UserId.ToString(), JsonConvert.SerializeObject(list));
                return model;
            }

            var oldModel = JsonConvert.DeserializeObject<List<Order>>(res);
            oldModel.Add(model);
            var options = new DistributedCacheEntryOptions().SetAbsoluteExpiration(DateTime.Now.AddMinutes(5)).SetSlidingExpiration(TimeSpan.FromMinutes(5));
            await _redisCache.SetStringAsync("Orders." + model.UserId.ToString(), JsonConvert.SerializeObject(oldModel), options);
            return model;
        }

        public async Task<IEnumerable<Order>> Add(IEnumerable<Order> models)
        {
            var addCarts = models as Order[] ?? models.ToArray();
            foreach (var model in addCarts)
            {
                await this.Add(model);
            }

            return addCarts;
        }

        public async Task<bool> Delete(string userId)
        {
            await _redisCache.RemoveAsync("Orders." + userId);
            return true;
        }
    }
}