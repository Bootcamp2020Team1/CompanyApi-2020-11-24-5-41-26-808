using System;
using System.Collections.Generic;
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
            client.DeleteAsync("companies/Clear");
        }

        [Fact]
        public async Task Should_add_new_company()
        {
            // given
            Company company = new Company(name: "Apple");
            string request = JsonConvert.SerializeObject(company);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");

            // when
            var response = await client.PostAsync("companies", requestBody);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Company actualCompany = JsonConvert.DeserializeObject<Company>(responseString);

            // then
            Assert.Equal(company, actualCompany);
        }

        [Fact]
        public async Task Should_get_all_company()
        {
            // given
            Company company = new Company(name: "Apple");
            string request = JsonConvert.SerializeObject(company);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");
            await client.PostAsync("companies", requestBody);

            // when
            var response = await client.GetAsync("companies");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            List<Company> actualCompany = JsonConvert.DeserializeObject<List<Company>>(responseString);

            // then
            Assert.Equal(new List<Company> { company }, actualCompany);
        }

        [Fact]
        public async Task Should_get_company_by_name()
        {
            // given
            Company company = new Company(name: "Apple");
            string request = JsonConvert.SerializeObject(company);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");
            await client.PostAsync("companies", requestBody);

            // when
            var response = await client.GetAsync("companies/Apple");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Company actualCompany = JsonConvert.DeserializeObject<Company>(responseString);

            // then
            Assert.Equal(company, actualCompany);
        }

        [Fact]
        public async Task Should_query_company_by_pageSize_and_pageIndex()
        {
            // given
            Company company = new Company(name: "Apple");
            string request = JsonConvert.SerializeObject(company);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");
            await client.PostAsync("companies", requestBody);

            Company company2 = new Company(name: "Banana");
            string request2 = JsonConvert.SerializeObject(company2);
            StringContent requestBody2 = new StringContent(request2, Encoding.UTF8, "application/json");
            await client.PostAsync("companies", requestBody2);

            Company company3 = new Company(name: "Orange");
            string request3 = JsonConvert.SerializeObject(company3);
            StringContent requestBody3 = new StringContent(request3, Encoding.UTF8, "application/json");
            await client.PostAsync("companies", requestBody3);

            // when
            var response = await client.GetAsync("companies?pageSize=1&pageIndex=2");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            List<Company> actualCompany = JsonConvert.DeserializeObject<List<Company>>(responseString);

            // then
            Assert.Equal(new List<Company> { company2 }, actualCompany);

            // when
            var response2 = await client.GetAsync("companies?pageSize=2&pageIndex=2");
            response.EnsureSuccessStatusCode();
            var responseString2 = await response2.Content.ReadAsStringAsync();
            List<Company> actualCompany2 = JsonConvert.DeserializeObject<List<Company>>(responseString2);

            // then
            Assert.Equal(new List<Company> { company3 }, actualCompany2);
        }
    }
}
