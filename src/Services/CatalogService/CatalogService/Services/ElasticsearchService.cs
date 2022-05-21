using CatalogService.ElasticSearchOptions;
using CatalogService.Entities;
using CatalogService.Services.Interfaces;

using Microsoft.Extensions.Configuration;

using Nest;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogService.Services
{
    public class ElasticsearchService : IElasticsearchService
    {
        private readonly IConfiguration _configuration;
        private readonly IElasticClient _client;

        public ElasticsearchService(IConfiguration configuration)
        {
            _configuration = configuration;
            _client = CreateInstance();
        }

        private ElasticClient CreateInstance()
        {
            string host = _configuration.GetSection("ElasticsearchServer:Host").Value;
            string username = _configuration.GetSection("ElasticsearchServer:Username").Value;
            string password = _configuration.GetSection("ElasticsearchServer:Password").Value;

            var settings = new ConnectionSettings(new Uri(host));

            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                settings.BasicAuthentication(username, password);

            return new ElasticClient(settings);
        }

        public async Task ChekIndex(string indexName)
        {
            var anyy = await _client.Indices.ExistsAsync(indexName);
            if (anyy.Exists)
                return;

            var response = await _client.Indices.CreateAsync(indexName,
                ci => ci
                    .Index(indexName)
                    .ProductMapping()
                    .Settings(s => s.NumberOfShards(3).NumberOfReplicas(1))
                    );

            return;
        }

        public async Task InsertDocument(string indexName, Product product)
        {
            var response = await _client.CreateAsync(product, q => q.Index(indexName));
            if (response.ApiCall?.HttpStatusCode == 409)
            {
                await _client.UpdateAsync<Product>(response.Id, a => a.Index(indexName).Doc(product));
            }
        }

        public async Task InsertDocuments(string indexName, List<Product> products)
        {
            await _client.IndexManyAsync(products, index: indexName);
        }

        public async Task<Product> GetDocument(string indexName, int id)
        {
            var response = await _client.GetAsync<Product>(id, q => q.Index(indexName));

            return response.Source;
        }

        public async Task<List<Product>> GetDocuments(string indexName)
        {
            var response = await _client.SearchAsync<Product>(q => q.Index(indexName).Scroll("5m"));
            return response.Documents.ToList();
        }
    }
}