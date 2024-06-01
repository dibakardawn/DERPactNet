using Microsoft.VisualStudio.TestTools.UnitTesting;
using PactNet;
using PactNet.Matchers;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace DERPactNet
{
    [TestClass]
    public class ConsumerPactTest
    {
        private IPactBuilderV3 pactBuilder;

        [TestMethod]
        public async Task EnsureProducerHonorsContract()
        {
            var pact = Pact.V3("Consumer API", "Provider API", new PactConfig());
            pactBuilder = pact.WithHttpInteractions();

            pactBuilder
                .UponReceiving("A GET request to /api/data")
                    .Given("There is available data")
                    .WithRequest(HttpMethod.Post, "/api/data")
                    /*.WithHeader("Content-Type", "application/json; charset=utf-8")
                    .WithJsonBody(new
                    {
                        id = 10
                    })*/
                .WillRespond()
                    .WithStatus(HttpStatusCode.OK)
                    .WithHeader("Content-Type", "application/json; charset=utf-8")
                    .WithJsonBody(new TypeMatcher(new
                    {
                        data = new List<string> { "Value1", "Value2" }
                    }));

            await pactBuilder.VerifyAsync(async ctx =>
            {
                var httpClient = new HttpClient();
                httpClient.BaseAddress = ctx.MockServerUri;
                var provider = new ProviderService(httpClient);
                var response = await provider.GetData();
                Assert.IsNotNull(response);
            });
        }
    }
}
