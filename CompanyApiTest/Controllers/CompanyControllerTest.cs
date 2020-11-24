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
            var getResponse = await client.GetAsync("company");
            response.EnsureSuccessStatusCode();
            var responseString = await getResponse.Content.ReadAsStringAsync();
            List<Company> actualCompanyList = JsonConvert.DeserializeObject<List<Company>>(responseString);
            // then
            Assert.Equal(1, actualCompanyList[0].CompanyId);
        }
    }
}
