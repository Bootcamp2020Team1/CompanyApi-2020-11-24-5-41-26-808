﻿using System;
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
            CompanyUpdateModel companyUpdateModel = new CompanyUpdateModel(name: "Apple");
            string request = JsonConvert.SerializeObject(companyUpdateModel);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");

            // when
            var response = await client.PostAsync("companies", requestBody);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Company actualCompany = JsonConvert.DeserializeObject<Company>(responseString);

            // then
            Assert.Equal(new Company(name: "Apple"), actualCompany);
        }

        [Fact]
        public async Task Should_get_all_company()
        {
            // given
            CompanyUpdateModel companyUpdateModel = new CompanyUpdateModel(name: "Apple");
            string request = JsonConvert.SerializeObject(companyUpdateModel);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");
            await client.PostAsync("companies", requestBody);

            // when
            var response = await client.GetAsync("companies");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            List<Company> actualCompany = JsonConvert.DeserializeObject<List<Company>>(responseString);

            // then
            Assert.Equal(new List<Company> { new Company(name: "Apple") }, actualCompany);
        }

        [Fact]
        public async Task Should_get_company_by_name()
        {
            // given
            CompanyUpdateModel companyUpdateModel = new CompanyUpdateModel(name: "Apple");
            string request = JsonConvert.SerializeObject(companyUpdateModel);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");
            await client.PostAsync("companies", requestBody);

            // when
            var response = await client.GetAsync("companies/Apple");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Company actualCompany = JsonConvert.DeserializeObject<Company>(responseString);

            // then
            Assert.Equal(new Company(name: "Apple"), actualCompany);
        }

        [Fact]
        public async Task Should_query_company_by_pageSize_and_pageIndex()
        {
            // given
            CompanyUpdateModel companyUpdateModel = new CompanyUpdateModel(name: "Apple");
            string request = JsonConvert.SerializeObject(companyUpdateModel);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");
            await client.PostAsync("companies", requestBody);

            CompanyUpdateModel companyUpdateModel2 = new CompanyUpdateModel(name: "Banana");
            string request2 = JsonConvert.SerializeObject(companyUpdateModel2);
            StringContent requestBody2 = new StringContent(request2, Encoding.UTF8, "application/json");
            await client.PostAsync("companies", requestBody2);

            CompanyUpdateModel companyUpdateModel3 = new CompanyUpdateModel(name: "Orange");
            string request3 = JsonConvert.SerializeObject(companyUpdateModel3);
            StringContent requestBody3 = new StringContent(request3, Encoding.UTF8, "application/json");
            await client.PostAsync("companies", requestBody3);

            // when
            var response = await client.GetAsync("companies?pageSize=1&pageIndex=2");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            List<Company> actualCompany = JsonConvert.DeserializeObject<List<Company>>(responseString);

            // then
            Assert.Equal(new List<Company> { new Company(name: "Banana") }, actualCompany);

            // when
            var response2 = await client.GetAsync("companies?pageSize=2&pageIndex=2");
            response.EnsureSuccessStatusCode();
            var responseString2 = await response2.Content.ReadAsStringAsync();
            List<Company> actualCompany2 = JsonConvert.DeserializeObject<List<Company>>(responseString2);

            // then
            Assert.Equal(new List<Company> { new Company(name: "Orange") }, actualCompany2);
        }

        [Fact]
        public async Task Should_change_company_name()
        {
            // given
            CompanyUpdateModel companyUpdateModel = new CompanyUpdateModel(name: "Apple");
            string request = JsonConvert.SerializeObject(companyUpdateModel);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");
            await client.PostAsync("companies", requestBody);

            var getResponse = await client.GetAsync("companies/Apple");
            getResponse.EnsureSuccessStatusCode();
            var responseString = await getResponse.Content.ReadAsStringAsync();
            Company expectedCompany = JsonConvert.DeserializeObject<Company>(responseString);

            // when
            CompanyUpdateModel companyUpdateModel2 = new CompanyUpdateModel(name: "Banana");
            string request2 = JsonConvert.SerializeObject(companyUpdateModel2);
            StringContent requestBody2 = new StringContent(request2, Encoding.UTF8, "application/json");
            var patchResponse = await client.PatchAsync("companies/Apple", requestBody2);
            patchResponse.EnsureSuccessStatusCode();
            var responseString2 = await patchResponse.Content.ReadAsStringAsync();
            Company actualCompany = JsonConvert.DeserializeObject<Company>(responseString2);

            // then
            Assert.Equal(new Company(name: "Banana"), actualCompany);
            Assert.Equal(expectedCompany.Id, actualCompany.Id);
        }
    }
}
