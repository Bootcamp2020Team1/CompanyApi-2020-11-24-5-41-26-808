using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyApi.Models
{
    public class Employee
    {
        public Employee()
        {
        }

        public Employee(string id, string name, string salary, string companyId)
        {
            this.Id = id;
            this.Name = name;
            this.Salary = salary;
            this.CompanyId = companyId;
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Salary { get; set; }
        public string CompanyId { get; set; }

        public override bool Equals(object obj)
        {
            Employee other = (Employee)obj;
            return Name == other.Name && Id == other.Id && Salary == other.Salary && CompanyId == other.CompanyId;
        }
    }
}
