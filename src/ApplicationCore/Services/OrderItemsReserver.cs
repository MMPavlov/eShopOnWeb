using System.Collections.Generic;
using Ardalis.GuardClauses;
using BlazorShared;
using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.ApplicationCore.Entities.BasketAggregate;
using Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.ApplicationCore.Specifications;
using System.Linq;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;
using JsonConverter = System.Text.Json.Serialization.JsonConverter;

namespace Microsoft.eShopWeb.ApplicationCore.Services
{
    public class OrderItemsReserver : IOrderItemsReserver
    {
        private readonly IUriComposer _uriComposer;
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;

        public OrderItemsReserver(IUriComposer uriComposer, HttpClient httpClient, BaseUrlConfiguration baseUrlConfiguration)
        {
            _uriComposer = uriComposer;
            _httpClient = httpClient;
            _apiUrl = baseUrlConfiguration.AzureFunctionBase;
        }

        public async Task CallFunctionAsync(Dictionary<string, int> orderDetails)
        {
            var itemOrder = new List<ItemOrder>();
            foreach (var entry in orderDetails)
            {
                itemOrder.Add(new ItemOrder
                {
                    Id = entry.Key,
                    Quantity = entry.Value
                });
            }
            var orderJson = new StringContent(JsonConvert.SerializeObject(itemOrder));
            await _httpClient.PostAsync($"{_apiUrl}", orderJson);
        }

        private class ItemOrder
        {
            [JsonPropertyName("id")]
            public string Id { get; set; }
            [JsonPropertyName("quantity")]
            public int Quantity { get; set; }
        }
    }
}