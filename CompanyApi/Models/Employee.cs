using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyApi.Models
{
    public class Employee
    {
        private string companyId;
        private string id;
        private string name;
        private string salary;
        public Employee(string id, string name, string salary, string companyId)
        {
            this.id = id;
            this.name = name;
            this.salary = salary;
            this.companyId = companyId;
        }
    }
}
