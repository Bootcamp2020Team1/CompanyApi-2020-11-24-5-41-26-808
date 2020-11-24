using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyApi.Models
{
    public class Company
    {
        //private List<Employee> employees = new List<Employee>();

        public Company()
        {
        }

        public Company(string name)
        {
            this.Id = Guid.NewGuid().ToString();
            this.Name = name;
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public override bool Equals(object obj)
        {
            Company other = (Company)obj;
            return Name == other.Name && Id == other.Id;
        }
    }
}
