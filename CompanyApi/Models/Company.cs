using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyApi.Models
{
    public class Company
    {
        private List<Employee> employees = new List<Employee>();
        private string id;
        private string name;
        public Company() 
        { 
        }

        public Company(string id, string name)
        {
            //this.id = Guid.NewGuid().ToString();
            this.id = id;
            this.name = name;
        }

        public override bool Equals(object obj)
        {
            Company other = (Company)obj;
            return name == other.name && id == other.id;
        }
    }
}
