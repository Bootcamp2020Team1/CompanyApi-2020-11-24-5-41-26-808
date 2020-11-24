using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CompanyApi;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using Xunit;

namespace CompanyApiTest.Controllers
{
    public class CompanyControllerTest
    {
        private readonly TestServer server = new TestServer(new WebHostBuilder()
            .UseStartup<Startup>());
        private readonly HttpClient client;

        public CompanyControllerTest()
        {
            client = server.CreateClient();
            client.DeleteAsync("company/clear");
        }

        [Fact]
        public async Task Should_Add_Company_When_Add_Company()
        {
            // given
            Company company = new Company("testCompany");
            string request = JsonConvert.SerializeObject(company);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");

            // when
            var response = await client.PostAsync("company", requestBody);
            var getResponse = await client.GetAsync("/company/companies");
            response.EnsureSuccessStatusCode();
            var responseString = await getResponse.Content.ReadAsStringAsync();
            List<Company> actualCompanyList = JsonConvert.DeserializeObject<List<Company>>(responseString);
            // then
            Assert.Equal(1, actualCompanyList[0].CompanyId);
        }

        [Fact]
        public async Task Should_Return_Company_When_Get_By_CompanyID()
        {
            // given
            Company company = new Company("testCompany");
            string request = JsonConvert.SerializeObject(company);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");

            // when
            var response = await client.PostAsync("company", requestBody);
            var getResponse = await client.GetAsync("company/1");
            getResponse.EnsureSuccessStatusCode();
            var responseString = await getResponse.Content.ReadAsStringAsync();
            Company actualCompany = JsonConvert.DeserializeObject<Company>(responseString);
            // then
            Assert.Equal(1, actualCompany.CompanyId);
        }

        [Fact]
        public async Task Should_Return_Correct_Company_List_When_Get_By_Index_Range()
        {
            // given
            Company company1 = new Company("testCompany");
            string request1 = JsonConvert.SerializeObject(company1);
            StringContent requestBody1 = new StringContent(request1, Encoding.UTF8, "application/json");

            Company company2 = new Company("testCompany");
            string request2 = JsonConvert.SerializeObject(company2);
            StringContent requestBody2 = new StringContent(request2, Encoding.UTF8, "application/json");

            // when
            var response1 = await client.PostAsync("company", requestBody1);
            var response2 = await client.PostAsync("company", requestBody2);
            var getResponse = await client.GetAsync("company?pagesize=1&pageindex=2");
            getResponse.EnsureSuccessStatusCode();
            var responseString = await getResponse.Content.ReadAsStringAsync();
            List<Company> actualCompanyList = JsonConvert.DeserializeObject<List<Company>>(responseString);
            // then
            Assert.Equal(2, actualCompanyList[0].CompanyId);
        }
    }
}
