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
    [Route("Companies/{companyID}/Employees")]
    public class EmployeeApi : ControllerBase
    {
        [HttpPost]
        public ActionResult<Company> AddNewEmployee(string companyID, Employee employee)
        {
            var company = FakeDatabase.GetCompanyByID(companyID);

            if (company == null)
            {
                return NotFound();
            }

            employee.EmployeeID = Guid.NewGuid().ToString();
            if (company.Employees == null)
            {
                company.Employees = new List<Employee>();
            }

            company.Employees.Add(employee);
            var response = new ObjectResult(employee)
            {
                StatusCode = (int)HttpStatusCode.OK,
            };

            Response.Headers.Add("Location", $"/Companies/{company.CompanyID}/Employees/{employee.EmployeeID}");
            return response;
        }
    }
}
