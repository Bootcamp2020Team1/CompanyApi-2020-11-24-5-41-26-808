using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CompanyApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CompanyApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CompaniesController : ControllerBase
    {
        private static List<Company> companies = new List<Company>();

        [HttpDelete("clear")]
        public void DeletePet()
        {
            companies.Clear();
        }

        [HttpPost]
        public IActionResult AddCompany(Company newCompany)
        {
            bool isExist = companies.Any(item => item.Name == newCompany.Name);
            if (isExist)
            {
                return Conflict();
            }

            companies.Add(newCompany);
            return CreatedAtAction(nameof(GetCompanById), new { id = newCompany.Id }, newCompany);
        }

        [HttpGet]
        public IActionResult GetXCompaniesFromPageY(int? pageSize, int? pageIndex)
        {
            var companyList = companies.Where((company, index) =>
                (pageSize == null || (pageIndex == null ||
                (index >= pageSize * (pageIndex - 1) &&
                index < pageSize * pageIndex))));

            return Ok(companyList);
        }

        [HttpGet("{id}")]
        public IActionResult GetCompanById(string id)
        {
            Company findCompany = companies.FirstOrDefault(company => company.Id == id);
            if (findCompany == null)
            {
                return NotFound();
            }

            return Ok(findCompany);
        }

        [HttpPatch("{id}")]
        public IActionResult UpdateCompanById(string id, UpdateCompany updateCompany)
        {
            var neetToUpdate = companies.First(company => company.Id == id);
            if (neetToUpdate == null)
            {
                return NotFound();
            }

            neetToUpdate.Name = updateCompany.Name;
            return Ok(neetToUpdate);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCompanyById(string id)
        {
            var findCompany = companies.FirstOrDefault(company => company.Id == id);
            if (findCompany == null)
            {
                return NotFound();
            }

            companies.Remove(findCompany);
            return Ok(findCompany);
        }

        [HttpPost("{id}/Employees")]
        public IActionResult AddEmployee(string id, Employee newEmployee)
        {
            var company = companies.FirstOrDefault(company => company.Id == id);
            var employees = company.Employees;
            var findEmploy = employees.FirstOrDefault(employee => employee.Id == newEmployee.Id);
            if (findEmploy == null)
            {
                employees.Add(newEmployee);
                var addedEmploy = employees.FirstOrDefault(employee => employee.Id == newEmployee.Id);
                return Ok(newEmployee);
            }

            return Conflict();
        }

        [HttpGet("{id}/Employees")]
        public IEnumerable<Employee> GetAllEmployee(string id)
        {
            var company = companies.FirstOrDefault(company => company.Id == id);
            return company.Employees;
        }

        [HttpPatch("{companyId}/Employees/{employeeId}")]
        public IActionResult UpdateEmployeeById(string companyId, string employeeId, UpdateEmployee updateEmployee)
        {
            var company = companies.FirstOrDefault(company => company.Id == companyId);
            var employees = company.Employees;
            var findEmploy = employees.FirstOrDefault(employee => employee.Id == employeeId);
            if (findEmploy != null)
            {
                findEmploy.Salary = updateEmployee.Salary;
                var afterEmploy = employees.FirstOrDefault(employee => employee.Id == employeeId);
                return Ok(afterEmploy);
            }

            return NotFound();
        }

        [HttpDelete("{companyId}/Employees/{employeeId}")]
        public IActionResult DeleteEmployeeById(string companyId, string employeeId)
        {
            var company = companies.FirstOrDefault(company => company.Id == companyId);
            var employees = company.Employees;
            var findEmploy = employees.FirstOrDefault(employee => employee.Id == employeeId);
            if (findEmploy == null)
            {
                return NotFound();
            }

            employees.Remove(findEmploy);
            return Ok(findEmploy);
        }
    }
}
