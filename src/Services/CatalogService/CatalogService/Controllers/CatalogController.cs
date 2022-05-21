using AutoMapper;

using CatalogService.Entities;
using CatalogService.Models;
using CatalogService.Repositories.Interfaces;
using CatalogService.Services.Interfaces;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CatalogService.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    public class CatalogController : ControllerBase
    {
        private readonly IProductRepository _repository;
        private readonly ILogger<CatalogController> _logger;
        private readonly IMapper _mapper;
        private readonly IElasticsearchService _elasticsearchService;

        public CatalogController(IProductRepository repository, ILogger<CatalogController> logger, IMapper mapper, IElasticsearchService elasticsearchService)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _elasticsearchService = elasticsearchService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProductViewModel>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<ProductViewModel>>> GetProducts()
        {
            var res = await _elasticsearchService.GetDocuments("product");
            if (!res.Any()) return Ok();

            var resmodel = _mapper.Map<List<ProductViewModel>>(res);
            return Ok(resmodel);
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<ProductViewModel>>> SetElasticsearchIndex()
        {
            await _elasticsearchService.ChekIndex("product");
            var products = await _repository.GetAll().ToListAsync();

            if (!products.Any()) return Ok();

            await _elasticsearchService.InsertDocuments("product", products);
            return Ok();
        }

        [HttpGet("{id:int}", Name = "GetProduct")]
        [ProducesResponseType(typeof(ProductViewModel), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ProductViewModel>> GetProduct(int id)
        {
            var res = await _repository.Get(m => m.Id == id);
            return Ok(res);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ProductSaveModel), (int)HttpStatusCode.Created)]
        public async Task<ActionResult<ProductSaveModel>> CreateProduct([FromBody] ProductSaveModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var entity = _mapper.Map<Product>(model);

            await _repository.Insert(entity);

            model = _mapper.Map<ProductSaveModel>(entity);

            return CreatedAtRoute("GetProduct", new { id = model.Id }, model);
        }

        [HttpPut]
        [ProducesResponseType(typeof(ProductSaveModel), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> UpdateProduct([FromBody] ProductSaveModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var entity = _mapper.Map<Product>(model);
            return Ok(await _repository.Update(entity));
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(typeof(ProductSaveModel), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> DeleteProductById(int id)
        {
            return Ok(await _repository.Delete(id));
        }
    }
}