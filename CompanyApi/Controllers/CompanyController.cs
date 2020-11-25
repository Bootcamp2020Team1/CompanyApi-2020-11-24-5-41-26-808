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
        public async Task<ActionResult<IEnumerable<Company>>> GetAllCompanies()
        {
            return Ok(companies);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Company>> GetCompanyById(int id)
        {
            return Ok(companies.FirstOrDefault(company => company.CompanyId == id));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Company>>> GetCompaniesByRange(int pagesize, int pageindex)
        {
            return Ok(companies.Where(company => company.CompanyId >= pagesize * pageindex &&
                                                 company.CompanyId < (pagesize + 1) * pageindex).ToList());
        }

        [HttpGet("{id}/employees")]
        public async Task<ActionResult<IEnumerable<Company>>> GetEmployeesByCompanyId(int id)
        {
            return Ok(companies.FirstOrDefault(company => company.CompanyId == id).Employees);
        }

        [HttpGet("{companyId}/employees/{employeeId}")]
        public async Task<ActionResult<Employee>> GetEmployeeByEmployeeId(int companyId, int employeeId)
        {
            return Ok(companies.FirstOrDefault(company => company.CompanyId == companyId)
                .Employees.FirstOrDefault(employee => employee.EmployeeID == employeeId));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Company>> UpdateCompanyName(int id, UpdateModel updateModel)
        {
            companies.FirstOrDefault(company => company.CompanyId == id).Name = updateModel.Name;
            return Ok(companies.FirstOrDefault(company => company.CompanyId == id));
        }

        [HttpPut("{companyId}/employees/{employeeId}")]
        public async Task<ActionResult<Employee>> UpdateEmployeeInfo(int companyId, int employeeId, EmployeeUpdateModel employeeUpdateModel)
        {
            var employee = companies.FirstOrDefault(company => company.CompanyId == companyId)
                .Employees.FirstOrDefault(employee => employee.EmployeeID == employeeId);
            employee.Salary = employeeUpdateModel.Salary;
            employee.Name = employeeUpdateModel.Name;
            return Ok(employee);
        }

        [HttpPost("{id}")]
        public async Task<ActionResult<Company>> AddEmployeeToCompany(int id, EmployeeUpdateModel employee)
        {
            companies.FirstOrDefault(company => company.CompanyId == id).AddEmployee(employee);
            return Ok(companies.FirstOrDefault(company => company.CompanyId == id));
        }

        [HttpPost]
        public async Task<ActionResult<Company>> AddCompany(UpdateModel company)
        {
            if (companies.Any(companyInList => companyInList.Name == company.Name))
            {
                return Conflict();
            }

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

            var addCompany = new Company(company.Name);
            addCompany.CompanyId = addCompanyID;
            companies.Add(addCompany);
            return Ok(addCompany);
        }

        [HttpDelete("clear")]
        public async Task<ActionResult> Clear()
        {
            companies.Clear();
            return Ok();
        }

        [HttpDelete("{companyId}/employees/{employeeId}")]
        public async Task<ActionResult> DeleteEmployee(int companyId, int employeeId)
        {
            companies.FirstOrDefault(company => company.CompanyId == companyId).Employees
                .Remove(companies.FirstOrDefault(company => company.CompanyId == companyId).Employees
                .FirstOrDefault(employee => employee.EmployeeID == employeeId));
            return Ok();
        }

        [HttpDelete("{companyId}")]
        public async Task<ActionResult> DeleteCompany(int companyId)
        {
            companies.Remove(companies.FirstOrDefault(company => company.CompanyId == companyId));
            return Ok();
        }
    }
}
