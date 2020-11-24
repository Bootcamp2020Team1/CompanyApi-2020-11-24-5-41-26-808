﻿using System;
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

        [HttpGet]
        public IEnumerable<Company> GetAllCompanies()
        {
            return companies;
        }

        [HttpGet("{id}")]
        public Company GetCompanById(string id)
        {
            return companies.First(company => company.Id == id);
        }

        [HttpPatch("{id}")]
        public Company UpdateCompanById(string id, UpdateCompany updateCompany)
        {
            var neetToUpdate = companies.First(company => company.Id == id);
            neetToUpdate.Name = updateCompany.Name;
            return neetToUpdate;
        }

        [HttpPost]
        public Company AddCompany(Company newCompany)
        {
            companies.Add(newCompany);
            return newCompany;
        }
    }
}
