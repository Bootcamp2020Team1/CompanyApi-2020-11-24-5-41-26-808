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

        public async Task<Company> DeserializeResponseToCompanyAsync(HttpResponseMessage response)
        {
            var responseString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Company>(responseString);
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

            var responseString = await response.Content.ReadAsStringAsync();
            var actualCompany = JsonConvert.DeserializeObject<Company>(responseString);
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
    }
}