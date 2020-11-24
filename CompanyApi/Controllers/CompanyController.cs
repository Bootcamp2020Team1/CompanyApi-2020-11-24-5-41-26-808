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
        [HttpGet]
        public IList<Company> Get()
        {
            return companies;
        }

        [HttpPost]
        public Company AddCompany(Company company)
        {
            company.CompanyId = companies.Count + 1;
            companies.Add(company);
            return company;
        }
    }
}
