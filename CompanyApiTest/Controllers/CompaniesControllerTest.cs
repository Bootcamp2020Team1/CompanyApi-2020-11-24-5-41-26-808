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

using System.Collections.Generic;

namespace CompanyApiTest.Controllers
{
    public class CompaniesControllerTest
    {
        // given
        private TestServer server;
        private HttpClient client;
        public CompaniesControllerTest()
        {
            this.server = new TestServer(new WebHostBuilder()
               .UseStartup<Startup>());
            this.client = server.CreateClient();
        }

        [Fact]
        public async Task Should_return_added_company_when_add_company_successfully()
        {
            await client.DeleteAsync("Companies/clear");
            var company = new Company("Sun");
            string request = JsonConvert.SerializeObject(company);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");

            //when
            var response = await client.PostAsync("/Companies", requestBody);

            //then
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Company actual = JsonConvert.DeserializeObject<Company>(responseString);
            Assert.Equal(company, actual);
        }

        [Fact]
        public async Task Should_return_all_companies_when_get()
        {
            await client.DeleteAsync("Companies/clear");
            var company1 = new Company("Sun");
            var company2 = new Company("Moon");
            string request1 = JsonConvert.SerializeObject(company1);
            string request2 = JsonConvert.SerializeObject(company2);
            StringContent requestBody1 = new StringContent(request1, Encoding.UTF8, "application/json");
            StringContent requestBody2 = new StringContent(request2, Encoding.UTF8, "application/json");

            //when
            await client.PostAsync("/Companies", requestBody1);
            await client.PostAsync("/Companies", requestBody2);
            var response = await client.GetAsync("/Companies");

            //then
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            List<Company> actual = JsonConvert.DeserializeObject<List<Company>>(responseString);
            Assert.Equal(new List<Company> { company1, company2 }, actual);
        }

        [Fact]
        public async Task Should_return_company_when_get_company_by_id()
        {
            await client.DeleteAsync("Companies/clear");
            var company1 = new Company("Sun");
            var company2 = new Company("Moon");
            string request1 = JsonConvert.SerializeObject(company1);
            string request2 = JsonConvert.SerializeObject(company2);
            StringContent requestBody1 = new StringContent(request1, Encoding.UTF8, "application/json");
            StringContent requestBody2 = new StringContent(request2, Encoding.UTF8, "application/json");

            //when
            var responseWithId = await client.PostAsync("/Companies", requestBody1);
            responseWithId.EnsureSuccessStatusCode();
            var responseStringWithId = await responseWithId.Content.ReadAsStringAsync();
            Company actualWithId = JsonConvert.DeserializeObject<Company>(responseStringWithId);
            var id = actualWithId.Id;
            var response = await client.GetAsync($"/Companies/{id}");
            //then
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Company actual = JsonConvert.DeserializeObject<Company>(responseString);
            Assert.Equal(company1, actual);
        }
    }
}
