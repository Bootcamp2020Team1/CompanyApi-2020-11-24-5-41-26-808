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

        public StringContent Serialize<T>(T company)
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
            var requestBody = Serialize<CompanyPostModel>(company);

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
            var requestBody = Serialize(company);

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
            var requestBody = Serialize(company);
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
            var requestBody = Serialize(company);
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
            var requestBody = Serialize(company);
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

            var requestBody1 = Serialize(company1);
            var requestBody2 = Serialize(company2);
            var requestBody3 = Serialize(company3);

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

        [Fact]
        public async Task Should_Return_Updated_Company_Given_Existed_Company_Id_And_Updated_Information_When_Patch()
        {
            //given
            FakeDatabase.ClearCompanies();
            var company = new CompanyPostModel("company1");
            var requestBody = Serialize(company);
            var uri = "/Companies";
            var postResponse = await client.PostAsync(uri, requestBody);
            var existedCompany = await DeserializeResponseAsync<Company>(postResponse);

            var updatedCompany = new CompanyPostModel("companyUpdated");
            var requestBodyPatch = Serialize(updatedCompany);

            // when
            var response = await client.PatchAsync($"/Companies/{existedCompany.CompanyID}", requestBodyPatch);

            // then
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var actualCompany = await DeserializeResponseAsync<Company>(response);

            var responseGet = await client.GetAsync($"/Companies/{existedCompany.CompanyID}");
            var actualCompanyFromDatabase = await DeserializeResponseAsync<Company>(responseGet);

            existedCompany.Name = "companyUpdated";
            Assert.Equal(existedCompany, actualCompany);
            Assert.Equal(existedCompany, actualCompanyFromDatabase);
        }

        [Fact]
        public async Task Should_Return_Not_Found_Given_Not_Existed_Company_Id_And_Updated_Information_When_Patch()
        {
            //given
            FakeDatabase.ClearCompanies();
            var company = new CompanyPostModel("company1");
            var requestBody = Serialize(company);
            var uri = "/Companies";
            var postResponse = await client.PostAsync(uri, requestBody);
            var existedCompany = await DeserializeResponseAsync<Company>(postResponse);

            var updatedCompany = new CompanyPostModel("companyUpdated");
            var requestBodyPatch = Serialize(updatedCompany);

            // when
            var response = await client.PatchAsync($"/Companies/NOTFOUND", requestBodyPatch);

            // then
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Should_Add_Employee_Given_Company_ID_And_Employee_When_Post()
        {
            // given
            FakeDatabase.ClearCompanies();
            var company = new CompanyPostModel("company1");
            var requestBody = Serialize(company);
            var postResponse = await client.PostAsync("/Companies", requestBody);
            var existedCompany = await DeserializeResponseAsync<Company>(postResponse);

            var employee = new EmployeeUpdatedModel("employee1", 1300);
            var requestBodyPost = Serialize<EmployeeUpdatedModel>(employee);

            // when
            var response = await client.PostAsync($"/Companies/{existedCompany.CompanyID}/Employees", requestBodyPost);

            // then
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var actualEmployee = await DeserializeResponseAsync<Employee>(response);
            Assert.Equal(response.Headers.Location.ToString(), $"/Companies/{existedCompany.CompanyID}/Employees/{actualEmployee.EmployeeID}");
            Assert.Equal(employee.Name, actualEmployee.Name);
            Assert.Equal(employee.Salary, actualEmployee.Salary);
        }

        [Fact]
        public async Task Should_Return_Not_Found_Given_Not_Existed_Company_ID_And_Employee_When_Post()
        {
            // given
            FakeDatabase.ClearCompanies();
            var company = new CompanyPostModel("company1");
            var requestBody = Serialize(company);
            var postResponse = await client.PostAsync("/Companies", requestBody);
            var existedCompany = await DeserializeResponseAsync<Company>(postResponse);

            var employee = new EmployeeUpdatedModel("employee1", 1300);
            var requestBodyPost = Serialize<EmployeeUpdatedModel>(employee);

            // when
            var response = await client.PostAsync($"/Companies/notexisted/Employees", requestBodyPost);

            // then
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Should_Get_All_Employee_Given_Company_ID_When_Get()
        {
            // given
            FakeDatabase.ClearCompanies();
            var company = new CompanyPostModel("company1");
            var requestBody = Serialize(company);
            var postResponse = await client.PostAsync("/Companies", requestBody);
            var existedCompany = await DeserializeResponseAsync<Company>(postResponse);
            var employee = new EmployeeUpdatedModel("employee1", 1300);
            var requestBodyPost = Serialize<EmployeeUpdatedModel>(employee);
            await client.PostAsync($"/Companies/{existedCompany.CompanyID}/Employees", requestBodyPost);

            // when
            var response = await client.GetAsync($"/Companies/{existedCompany.CompanyID}/Employees");

            // then
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var actualEmployee = await DeserializeResponseAsync<IList<Employee>>(response);

            Assert.Equal(FakeDatabase.GetCompanyByID(existedCompany.CompanyID).Employees, actualEmployee);
        }

        [Fact]
        public async Task Should_Return_Not_Found_Given_Not_Existed_Company_ID_When_Get()
        {
            // given
            FakeDatabase.ClearCompanies();
            var company = new CompanyPostModel("company1");
            var requestBody = Serialize(company);
            var postResponse = await client.PostAsync("/Companies", requestBody);
            await DeserializeResponseAsync<Company>(postResponse);

            // when
            var response = await client.GetAsync($"/Companies/notexisted/Employees");

            // then
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}