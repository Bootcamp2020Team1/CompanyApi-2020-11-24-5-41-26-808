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

        [HttpPatch("{id}")]
        public Company UpdateCompanyName(int id, UpdateModel updateModel)
        {
            companies.FirstOrDefault(company => company.CompanyId == id).Name = updateModel.Name;
            return companies.FirstOrDefault(company => company.CompanyId == id);
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
            company.CompanyId = companies.Count + 1;
            companies.Add(company);
            return company;
        }

        [HttpDelete("clear")]
        public void Clear()
        {
            companies.Clear();
        }
    }
}
