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
        [HttpGet]
        public string Get()
        {
            return "Hello World";
        }

        [HttpPost]
        public Company AddCompany(Company newCompany)
        {
            companies.Add(newCompany);
            return newCompany;
        }
    }
}
