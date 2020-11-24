using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CompanyApi.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CompanyApi.Controllers
{
    public static class FakeDatabase
    {
        public static IList<Company> Companies { get; } = new List<Company>();
        public static void ClearCompanies()
        {
            Companies.Clear();
        }
    }

    [ApiController]
    [Route("Companies")]
    public class CompanyApi : ControllerBase
    {
        [HttpPost]
        public ActionResult<Company> AddNewCompany(Company company)
        {
            if (FakeDatabase.Companies.FirstOrDefault(companyInMemory => companyInMemory.Name == company.Name) != null)
            {
                return Conflict();
            }

            company.CompanyID = Guid.NewGuid().ToString();
            FakeDatabase.Companies.Add(company);
  
            var response = new ObjectResult(company)
            {
                StatusCode = (int)HttpStatusCode.OK,
            };

            Response.Headers.Add("Location", $"/Companies/{company.CompanyID}");
            return response;
        }

        [HttpGet]
        public ActionResult<Company> GetAllCompanies()
        {
            var response = new ObjectResult(FakeDatabase.Companies)
            {
                StatusCode = (int)HttpStatusCode.OK,
            };
            return response;
        }
    }
}
