using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using CompanyApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CompanyApi.Controllers
{
    [ApiController]
    [Route("companies")]
    public class CompanyController : ControllerBase
    {
        private static readonly IList<Company> companies = new List<Company>();
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Company>>> QueryCompany(int? pageSize, int? pageIndex)
        {
            return Ok(companies.Where(c =>
                (pageSize == null || (pageIndex == null || 
                (companies.IndexOf(c) >= pageSize * (pageIndex - 1) &&
                companies.IndexOf(c) < pageSize * pageIndex)))));
        }

        [HttpGet("{companyName}")]
        public async Task<ActionResult<Company>> GetCompanyByName(string companyName)
        {
            return Ok(companies.Where(c => c.Name == companyName).FirstOrDefault());
        }

        [HttpPost]
        public async Task<ActionResult<Company>> AddCompany(CompanyUpdateModel companyUpdateModel)
        {
            var company = new Company(companyUpdateModel.Name);
            companies.Add(company);
            return Ok(company);
        }

        [HttpPatch("{companyName}")]
        public async Task<ActionResult<Company>> UpdateCompany(string companyName, CompanyUpdateModel companyUpdateModel)
        {
            var company = companies.Where(c => c.Name == companyName).FirstOrDefault();
            company.Name = companyUpdateModel.Name;
            return Ok(company);
        }

        [HttpDelete("{companyName}")]
        public async Task<ActionResult<Company>> DeleteCompany(string companyName)
        {
            companies.Remove(companies.Where(c => c.Name == companyName).FirstOrDefault());
            return NoContent();
        }

        [HttpGet("{companyName}/employees")]
        public async Task<ActionResult<IEnumerable<Employee>>> GetAllEmployees(string companyName)
        {
            var company = companies.FirstOrDefault(c => c.Name == companyName);
            if (company == null)
            {
                return NotFound();
            }

            return Ok(company.Employees);
        }

        [HttpPatch("{companyName}/employees/{employeeName}")]
        public async Task<ActionResult<Employee>> UpdateEmployee(string companyName, string employeeName, EmployeeUpdateModel employeeUpdateModel)
        {
            var company = companies.FirstOrDefault(c => c.Name == companyName);
            if (company == null)
            {
                return NotFound();
            }

            var employee = company.Employees.FirstOrDefault(employee => employee.Name == employeeName);
            if (employee == null)
            {
                return NotFound();
            }

            employee.Name = employeeUpdateModel.Name;
            employee.Salary = employeeUpdateModel.Salary;
            return Ok(employee);
        }

        [HttpDelete("{companyName}/employees/{employeeName}")]
        public async Task<ActionResult> DeleteEmployee(string companyName, string employeeName)
        {
            var company = companies.FirstOrDefault(c => c.Name == companyName);
            if (company == null)
            {
                return NotFound();
            }

            var employee = company.Employees.FirstOrDefault(employee => employee.Name == employeeName);
            if (employee == null)
            {
                return NotFound();
            }

            company.Employees.Remove(employee);
            return NoContent();
        }

        [HttpPost("{companyName}/employees")]
        public async Task<ActionResult<Employee>> AddEmployee(string companyName, EmployeeUpdateModel employeeUpdateModel)
        {
            var company = companies.FirstOrDefault(c => c.Name == companyName);
            var employee = new Employee(employeeUpdateModel.Name, employeeUpdateModel.Salary);
            company.Employees.Add(employee);
            return Ok(employee);
        }

        [HttpDelete("Clear")]
        public void Clear()
        {
            companies.Clear();
        }
    }
}
