using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyApi.Models
{
    public class Company
    {
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
        public List<Employee> Employees { get; set; } = new List<Employee>();
        //public List<Employee> GetEmployees()
        //{
        //    return employees;
        //}

        public override bool Equals(object obj)
        {
            if (!(obj is Company))
            {
                return false;
            }

            Company other = (Company)obj;
            return Name == other.Name && Id == other.Id;
        }
    }
}
