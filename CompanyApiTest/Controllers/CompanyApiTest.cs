using CompanyApi;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using CompanyApi.Model;
using Newtonsoft.Json;
using System.Text;
using System.Net;
using System.Collections.Generic;
using CompanyApi.Controllers;

namespace CompanyApiTest
{
    public class CompanyApiTest
    {
        private readonly HttpClient client;
        public CompanyApiTest()
        {
            TestServer server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            this.client = server.CreateClient();
        }

        public StringContent SerializeCompany(CompanyPostModel company)
        {
            string request = JsonConvert.SerializeObject(company);
            return new StringContent(request, Encoding.UTF8, "application/json");
        }

        public async Task<T> DeserializeResponseAsync<T>(HttpResponseMessage response)
        {
            var responseString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(responseString);
        }

        [Fact]
        public async Task Should_Add_Company_Given_Company_Name_Not_Existed_When_Post()
        {
            // given
            var company = new CompanyPostModel("company1");
            var requestBody = SerializeCompany(company);

            // when
            var uri = "/Companies";
            var response = await client.PostAsync(uri, requestBody);

            // then
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var actualCompany = await DeserializeResponseAsync<Company>(response);
            Assert.Equal(response.Headers.Location.ToString(), $"{uri}/{actualCompany.CompanyID}");
            Assert.Equal(company.Name, actualCompany.Name);
        }

        [Fact]
        public async Task Should_Return_Conflict_Given_Company_Name_Existed_When_Post()
        {
            // given
            var company = new CompanyPostModel("company1");
            var requestBody = SerializeCompany(company);

            // when
            var uri = "/Companies";
            var response = await client.PostAsync(uri, requestBody);

            // then
            Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        }

        [Fact]
        public async Task Should_Return_All_Company_List_When_Get()
        {
            //given
            FakeDatabase.ClearCompanies();
            var company = new CompanyPostModel("company1");
            var requestBody = SerializeCompany(company);
            var uri = "/Companies";
            await client.PostAsync(uri, requestBody);

            // when
            var response = await client.GetAsync(uri);

            // then
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var actualCompany = await DeserializeResponseAsync<IList<Company>>(response);
            Assert.Equal(FakeDatabase.Companies, actualCompany);
        }

        [Fact]
        public async Task Should_Return_Ok_And_Company_Given_Existed_Company_Id_When_Get_By_ID()
        {
            //given
            FakeDatabase.ClearCompanies();
            var company = new CompanyPostModel("company1");
            var requestBody = SerializeCompany(company);
            var uri = "/Companies";
            var postResponse = await client.PostAsync(uri, requestBody);
            var existedCompany = await DeserializeResponseAsync<Company>(postResponse);

            // when
            var response = await client.GetAsync($"/Companies/{existedCompany.CompanyID}");

            // then
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var actualCompany = await DeserializeResponseAsync<Company>(response);
            Assert.Equal(existedCompany, actualCompany);
        }

        [Fact]
        public async Task Should_Return_Not_Found_Given_Not_Existed_Company_Id_When_Get_By_ID()
        {
            //given
            FakeDatabase.ClearCompanies();
            var company = new CompanyPostModel("company1");
            var requestBody = SerializeCompany(company);
            var uri = "/Companies";
            var postResponse = await client.PostAsync(uri, requestBody);
            await DeserializeResponseAsync<Company>(postResponse);

            // when
            var response = await client.GetAsync($"/Companies/notExistedID");

            // then
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Should_Return_Company_List_On_Page_Index_Given_Page_Size_And_Page_Index_When_Get()
        {
            //given
            FakeDatabase.ClearCompanies();
            var company1 = new CompanyPostModel("company1");
            var company2 = new CompanyPostModel("company2");
            var company3 = new CompanyPostModel("company3");

            var requestBody1 = SerializeCompany(company1);
            var requestBody2 = SerializeCompany(company2);
            var requestBody3 = SerializeCompany(company3);

            var uri = "/Companies";
            await client.PostAsync(uri, requestBody1);
            await client.PostAsync(uri, requestBody2);
            var expectedResponse = await client.PostAsync(uri, requestBody3);
            var expectedCompany = await DeserializeResponseAsync<Company>(expectedResponse);

            // when
            var response = await client.GetAsync("/Companies?pageSize=2&pageIndex=2");

            // then
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var actualCompany = await DeserializeResponseAsync<IList<Company>>(response);
            Assert.Equal(new List<Company>() { expectedCompany }, actualCompany);
        }
    }
}