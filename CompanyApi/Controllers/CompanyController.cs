using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CompanyApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CompanyController : ControllerBase
    {
        private static IList<Company> companies = new List<Company>();
        [HttpGet("companies")]
        public IList<Company> GetAllCompanies()
        {
            return companies;
        }

        [HttpGet("{id}")]
        public Company GetCompanyById(int id)
        {
            return companies.FirstOrDefault(company => company.CompanyId == id);
        }

        [HttpGet]
        public IList<Company> GetCompaniesByRange(int pagesize, int pageindex)
        {
            return companies.Where(company => company.CompanyId >= pagesize * pageindex &&
                                              company.CompanyId < (pagesize + 1) * pageindex).ToList();
        }

        [HttpGet("{id}/employees")]
        public IList<Employee> GetEmployeesByCompanyId(int id)
        {
            return companies.FirstOrDefault(company => company.CompanyId == id).Employees;
        }

        [HttpGet("{companyId}/employees/{employeeId}")]
        public Employee GetEmployeeByEmployeeId(int companyId, int employeeId)
        {
            return GetEmployeesByCompanyId(companyId).FirstOrDefault(employee => employee.EmployeeID == employeeId);
        }

        [HttpPatch("{id}")]
        public Company UpdateCompanyName(int id, UpdateModel updateModel)
        {
            companies.FirstOrDefault(company => company.CompanyId == id).Name = updateModel.Name;
            return companies.FirstOrDefault(company => company.CompanyId == id);
        }

        [HttpPatch("{companyId}/employees/{employeeId}")]
        public Employee UpdateEmployeeInfo(int companyId, int employeeId, EmployeeUpdateModel employeeUpdateModel)
        {
            GetEmployeeByEmployeeId(companyId, employeeId).Salary = employeeUpdateModel.Salary;
            GetEmployeeByEmployeeId(companyId, employeeId).Name = employeeUpdateModel.Name;
            return GetEmployeeByEmployeeId(companyId, employeeId);
        }

        [HttpPost("{id}")]
        public Company AddEmployeeToCompany(int id, Employee employee)
        {
            companies.FirstOrDefault(company => company.CompanyId == id).AddEmployee(employee);
            return companies.FirstOrDefault(company => company.CompanyId == id);
        }

        [HttpPost]
        public Company AddCompany(Company company)
        {
            int addCompanyID = 1;
            if (companies.Count != 0)
            {
                var companyIDs = new List<int>();
                foreach (var companyInList in companies)
                {
                    companyIDs.Add(companyInList.CompanyId);
                }

                addCompanyID = companyIDs.Max() + 1;
            }

            company.CompanyId = addCompanyID;
            companies.Add(company);
            return company;
        }

        [HttpDelete("clear")]
        public void Clear()
        {
            companies.Clear();
        }

        [HttpDelete("{companyId}/employees/{employeeId}")]
        public void DeleteEmployee(int companyId, int employeeId)
        {
            GetEmployeesByCompanyId(companyId).Remove(GetEmployeesByCompanyId(companyId)
                .FirstOrDefault(employee => employee.EmployeeID == employeeId));
        }

        [HttpDelete("{companyId}")]
        public void DeleteCompany(int companyId)
        {
            companies.Remove(companies.FirstOrDefault(company => company.CompanyId == companyId));
        }
    }
}
