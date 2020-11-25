using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyApi.Models
{
    public class UpdateEmployee
    {
        public UpdateEmployee()
        {
        }

        public UpdateEmployee(string name, string salary)
        {
            this.Name = name;
            this.Salary = salary;
            //this.CompanyId = companyId;
        }

        public string Name { get; set; }
        public string Salary { get; set; }
        //public string CompanyId { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is UpdateEmployee))
            {
                return false;
            }

            UpdateEmployee other = (UpdateEmployee)obj;
            return Name == other.Name && Salary == other.Salary;
        }
    }
}
