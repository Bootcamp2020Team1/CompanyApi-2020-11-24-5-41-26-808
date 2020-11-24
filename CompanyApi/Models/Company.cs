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

        public Company(string id, string name)
        {
            //this.id = Guid.NewGuid().ToString();
            this.Id = id;
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
