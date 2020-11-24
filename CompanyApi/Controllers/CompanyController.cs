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
        public async Task<ActionResult<IEnumerable<Company>>> Query(int? pageSize, int? pageIndex)
        {
            return Ok(companies.Where(c =>
                (pageSize == null || (pageIndex == null || 
                (companies.IndexOf(c) >= pageSize * (pageIndex - 1) &&
                companies.IndexOf(c) < pageSize * pageIndex)))));
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<Company>> GetByName(string name)
        {
            return Ok(companies.Where(c => c.Name == name).FirstOrDefault());
        }

        [HttpPost]
        public async Task<ActionResult<Company>> AddCompany(Company company)
        {
            companies.Add(company);
            return Ok(company);
        }

        [HttpDelete("Clear")]
        public void Clear()
        {
            companies.Clear();
        }
    }
}
