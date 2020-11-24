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
    [ApiController]
    [Route("Companies")]
    public class CompanyApi : ControllerBase
    {
        private static IList<Company> companies = new List<Company>();

        [HttpPost]
        public ActionResult<Company> AddNewCompany(Company company)
        {
            if (companies.FirstOrDefault(companyInMemory => companyInMemory.Name == company.Name) != null)
            {
                return Conflict();
            }

            company.CompanyID = Guid.NewGuid().ToString();
            companies.Add(company);
  
            var response = new ObjectResult(company)
            {
                StatusCode = (int)HttpStatusCode.OK,
            };

            Response.Headers.Add("Location", $"/Companies/{company.CompanyID}");
            return response;
        }
    }
}
