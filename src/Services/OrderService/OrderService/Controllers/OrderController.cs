using AutoMapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using OrderService.Models;
using OrderService.Models.Interfaces;
using OrderService.Repositories.Interfaces;

using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace OrderService.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _repository;
        private readonly ILogger<OrderController> _logger;
        private readonly IMapper _mapper;
        private readonly IWorkContext _wctx;

        public OrderController(IOrderRepository repository, ILogger<OrderController> logger, IMapper mapper, IWorkContext wctx)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _wctx = wctx;
        }

        [HttpGet]
        [ProducesResponseType(typeof(OrderViewModel), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<OrderViewModel>> GetOrders()
        {
            var res = await _repository.Gets(_wctx.GetUserId().ToString());
            if (!res.Any()) return Ok();

            var model = _mapper.Map<List<OrderViewModel>>(res);
            return Ok(model);
        }
    }
}