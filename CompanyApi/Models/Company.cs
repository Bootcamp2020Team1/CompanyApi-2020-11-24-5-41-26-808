using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyApi.Models
{
    public class Company
    {
        private string id;
        private string name;
        public Company(string id, string name) 
        {
            this.id = id;
            this.name = name;
        }
    }
}
