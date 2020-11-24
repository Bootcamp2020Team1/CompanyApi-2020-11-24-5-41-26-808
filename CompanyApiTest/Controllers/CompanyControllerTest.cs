﻿using System;
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

        [Fact]
        public async Task Should_Update_Company_Name_When_Patch_By_ID()
        {
            // given
            Company company = new Company("testCompany");
            string request = JsonConvert.SerializeObject(company);
            var updateModel = new UpdateModel("patchname");
            string patchRequest = JsonConvert.SerializeObject(updateModel);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");
            StringContent patchRequestBody = new StringContent(patchRequest, Encoding.UTF8, "application/json");

            // when
            await client.PostAsync("company", requestBody);
            await client.PatchAsync("company/1", patchRequestBody);
            var getResponse = await client.GetAsync("company/1");
            getResponse.EnsureSuccessStatusCode();
            var responseString = await getResponse.Content.ReadAsStringAsync();
            Company actualCompany = JsonConvert.DeserializeObject<Company>(responseString);
            // then
            Assert.Equal("patchname", actualCompany.Name);
        }

        [Fact]
        public async Task Should_Add_Employee_To_Company_When_Add_()
        {
            // given
            Company company = new Company("testCompany");
            string request = JsonConvert.SerializeObject(company);
            var employee = new Employee("employeename", 10000);
            string postRequest = JsonConvert.SerializeObject(employee);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");
            StringContent postRequestBody = new StringContent(postRequest, Encoding.UTF8, "application/json");

            // when
            await client.PostAsync("company", requestBody);
            await client.PostAsync("company/1", postRequestBody);
            var getResponse = await client.GetAsync("company/1");
            getResponse.EnsureSuccessStatusCode();
            var responseString = await getResponse.Content.ReadAsStringAsync();
            Company actualCompany = JsonConvert.DeserializeObject<Company>(responseString);

            // then
            Assert.Single(actualCompany.Employees);
            Assert.Equal(10000, actualCompany.Employees[0].Salary);
        }

        [Fact]
        public async Task Should_Get_Employees_By_Company_Id()
        {
            // given
            Company company = new Company("testCompany");
            string request = JsonConvert.SerializeObject(company);
            var employee1 = new Employee("employeename1", 10000);
            var employee2 = new Employee("employeename2", 20000);
            string postRequest1 = JsonConvert.SerializeObject(employee1);
            string postRequest2 = JsonConvert.SerializeObject(employee2);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");
            StringContent postRequestBody1 = new StringContent(postRequest1, Encoding.UTF8, "application/json");
            StringContent postRequestBody2 = new StringContent(postRequest2, Encoding.UTF8, "application/json");

            // when
            await client.PostAsync("company", requestBody);
            await client.PostAsync("company/1", postRequestBody1);
            await client.PostAsync("company/1", postRequestBody2);
            var getResponse = await client.GetAsync("company/1/employees/2");
            getResponse.EnsureSuccessStatusCode();
            var responseString = await getResponse.Content.ReadAsStringAsync();
            Employee actualEmployee = JsonConvert.DeserializeObject<Employee>(responseString);

            // then
            Assert.Equal(20000, actualEmployee.Salary);
        }

        [Fact]
        public async Task Should_Update_Employee_Info_When_Patch()
        {
            // given
            Company company = new Company("testCompany");
            string request = JsonConvert.SerializeObject(company);
            var employee1 = new Employee("employeename1", 10000);
            string postRequest1 = JsonConvert.SerializeObject(employee1);
            StringContent postRequestBody1 = new StringContent(postRequest1, Encoding.UTF8, "application/json");

            var employeeUpdateModel = new EmployeeUpdateModel("employeepatchname", 5000);
            string patchRequest = JsonConvert.SerializeObject(employeeUpdateModel);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");
            StringContent patchRequestBody = new StringContent(patchRequest, Encoding.UTF8, "application/json");

            // when
            await client.PostAsync("company", requestBody);
            await client.PostAsync("company/1", postRequestBody1);
            await client.PatchAsync("company/1/employees/1", patchRequestBody);
            var getResponse = await client.GetAsync("company/1/employees/1");
            getResponse.EnsureSuccessStatusCode();
            var responseString = await getResponse.Content.ReadAsStringAsync();
            Employee actualEmployee = JsonConvert.DeserializeObject<Employee>(responseString);
            // then
            Assert.Equal(5000, actualEmployee.Salary);
        }

        [Fact]
        public async Task Should_Remove_Employee_When_Delete_By_Employee_Id()
        {
            // given
            Company company = new Company("testCompany");
            string request = JsonConvert.SerializeObject(company);
            var employee1 = new Employee("employeename1", 10000);
            var employee2 = new Employee("employeename2", 20000);
            string postRequest1 = JsonConvert.SerializeObject(employee1);
            string postRequest2 = JsonConvert.SerializeObject(employee2);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");
            StringContent postRequestBody1 = new StringContent(postRequest1, Encoding.UTF8, "application/json");
            StringContent postRequestBody2 = new StringContent(postRequest2, Encoding.UTF8, "application/json");

            // when
            await client.PostAsync("company", requestBody);
            await client.PostAsync("company/1", postRequestBody1);
            await client.PostAsync("company/1", postRequestBody2);
            await client.DeleteAsync("company/1/employees/2");
            var getResponse = await client.GetAsync("company/1/employees");
            getResponse.EnsureSuccessStatusCode();
            var responseString = await getResponse.Content.ReadAsStringAsync();
            List<Employee> actualEmployees = JsonConvert.DeserializeObject<List<Employee>>(responseString);

            // then
            Assert.Single(actualEmployees);
        }
    }
}
