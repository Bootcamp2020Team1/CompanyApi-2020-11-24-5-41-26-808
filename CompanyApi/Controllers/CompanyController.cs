﻿using System;
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
        public async Task<ActionResult<List<Company>>> Get()
        {
            return Ok(companies);
        }

        [HttpGet("{name}")]
        public Company GetByName(string name)
        {
            return companies.Where(c => c.Name == name).FirstOrDefault();
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
