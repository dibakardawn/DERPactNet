using DERPactNet.Models;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DERPactNet
{
    public class ProviderService
    {
        private readonly HttpClient _client;
        public ProviderService(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            if (_client.BaseAddress is null)
            {
                throw new ArgumentNullException(nameof(_client.BaseAddress));
            }
        }

        public async Task<ProviderResponse> GetData()
        {
            //var requestModel = "{ \"id\" : 10 }";
            //var stringContent = new StringContent(JsonConvert.SerializeObject(requestModel));
            var httpResponse = await _client.PostAsync("/api/data", null);
            if (httpResponse.IsSuccessStatusCode)
            {
                var responseObj = await httpResponse.Content.ReadAsStringAsync();
                //return JsonSerializer.Deserialize<ProviderResponse>(responseObj);
                return new ProviderResponse() { Data = Enumerable.Empty<string>() };
            }
            return new ProviderResponse() { Data = Enumerable.Empty<string>() };
        }
    }
}
