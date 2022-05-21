using AutoMapper;

using CartService.Entities;
using CartService.Models;
using CartService.Models.Interfaces;
using CartService.Repositories.Interfaces;
using CartService.Services.Interfaces;

using EventBusRabbitMQ.Contact;
using EventBusRabbitMQ.Core;
using EventBusRabbitMQ.Events;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CartService.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository _repository;
        private readonly ILogger<CartController> _logger;
        private readonly IWorkContext _wctx;
        private readonly IMapper _mapper;
        private readonly EventBusRabbitMqECommerce _eventBus;
        private readonly IEmailSender _emailSender;

        public CartController(ICartRepository repository, ILogger<CartController> logger, IWorkContext wctx, IMapper mapper, EventBusRabbitMqECommerce eventBus, IEmailSender emailSender)
        {
            _repository = repository;
            _logger = logger;
            _wctx = wctx;
            _mapper = mapper;
            _eventBus = eventBus;
            _emailSender = emailSender;
        }

        [HttpGet]
        [ProducesResponseType(typeof(CartViewModel), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<CartViewModel>> GetCarts()
        {
            var res = await _repository.GetCarts(_wctx.GetUserId().ToString());

            if (!res.Any()) return Ok();

            return Ok(res);
        }

        [HttpPost]
        [ProducesResponseType(typeof(CartSaveModel), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<CartSaveModel>> AddCart([FromForm] CartSaveModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var entity = _mapper.Map<Cart>(model);
            await _repository.AddCart(entity);
            return Ok("Status Success");
        }

        [HttpPost]
        [ProducesResponseType(typeof(IEnumerable<CartSaveModel>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<CartSaveModel>>> AddCarts(
            [FromForm] IEnumerable<CartSaveModel> model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var entities = _mapper.Map<List<Cart>>(model);
            await _repository.AddCarts(entities);
            return Ok("Status Success");
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> Checkout()
        {
            try
            {
                var carts = await _repository.GetCarts(_wctx.GetUserId().ToString());
                if (!carts.Any()) return Ok();

                var model = new CreateEventModel { Data = carts };
                _eventBus.Publish(EventBusConstants.CreateQueue, model);

                await _emailSender.SendMail(_wctx.GetEmail(), carts);
                return Ok("Status Success");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error Publishing integration from {AppName}", "CartService");
                return Ok();
            }
        }
    }
}