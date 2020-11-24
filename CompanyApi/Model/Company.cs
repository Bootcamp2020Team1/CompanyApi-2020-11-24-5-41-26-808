using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyApi.Model
{
    public class Company
    {
        public Company(string companyID, string name)
        {
            CompanyID = companyID;
            Name = name;
        }

        public Company()
        {
        }

        public string CompanyID { get; set; }

        public string Name { get; set; }
    }
}
