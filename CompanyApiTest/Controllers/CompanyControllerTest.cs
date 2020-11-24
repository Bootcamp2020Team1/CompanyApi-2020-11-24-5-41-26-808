using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CompanyApi;
using CompanyApi.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using Xunit;

namespace CompanyApiTest.Controllers
{
    public class CompanyControllerTest
    {
        private readonly TestServer server;
        private readonly HttpClient client;
        public CompanyControllerTest()
        {
            server = new TestServer(new WebHostBuilder()
               .UseStartup<Startup>());
            client = server.CreateClient();
        }

        [Fact]
        public async Task Should_return_hello_world_with_default_request()
        {
            // given
            Company company = new Company(name: "Baymax");
            string request = JsonConvert.SerializeObject(company);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");

            // when
            var response = await client.GetAsync("companies");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            // then
            Assert.Equal("Hello World", responseString);
        }
    }
}
