using CartService.Entities;
using CartService.Models.Interfaces;
using CartService.Repositories.Interfaces;

using Microsoft.Extensions.Caching.Distributed;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CartService.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly IDistributedCache _redisCache;
        private readonly IWorkContext _wctx;

        public CartRepository(IDistributedCache redisCache, IWorkContext wctx)
        {
            _redisCache = redisCache;
            _wctx = wctx;
        }

        public async Task<IEnumerable<Cart>> GetCarts(string userId)
        {
            var model = await _redisCache.GetStringAsync("Carts." + userId);
            if (model == null)
            {
                return new List<Cart>();
            }
            return JsonConvert.DeserializeObject<List<Cart>>(model);
        }

        public async Task<Cart> AddCart(Cart model)
        {
            model.UserId = _wctx.GetUserId();

            var cart = await _redisCache.GetStringAsync("Carts." + model.UserId.ToString());
            if (string.IsNullOrEmpty(cart))
            {
                var list = new List<Cart> { model };
                await _redisCache.SetStringAsync("Carts." + model.UserId.ToString(), JsonConvert.SerializeObject(list));
                return model;
            }

            var oldModel = JsonConvert.DeserializeObject<List<Cart>>(cart);
            oldModel.Add(model);
            var options = new DistributedCacheEntryOptions().SetAbsoluteExpiration(DateTime.Now.AddMinutes(30)).SetSlidingExpiration(TimeSpan.FromMinutes(2));
            await _redisCache.SetStringAsync("Carts." + model.UserId.ToString(), JsonConvert.SerializeObject(oldModel), options);
            return model;
        }

        public async Task<IEnumerable<Cart>> AddCarts(IEnumerable<Cart> models)
        {
            var addCarts = models as Cart[] ?? models.ToArray();
            foreach (var model in addCarts)
            {
                await this.AddCart(model);
            }

            return addCarts;
        }

        public async Task<bool> DeleteCart(string userId)
        {
            await _redisCache.RemoveAsync("Carts." + userId);
            return true;
        }
    }
}