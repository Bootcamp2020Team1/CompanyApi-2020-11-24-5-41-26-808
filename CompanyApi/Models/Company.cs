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
            Id = Guid.NewGuid().ToString();
            Name = name;
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public IList<Employee> Employees { get; set; } = new List<Employee>();
        public override bool Equals(object? obj)
        {
            if (obj == null || !(obj is Company))
            {
                return false;
            }

            Company company = (Company)obj;
            return company.Name == Name;
        }
    }
}
