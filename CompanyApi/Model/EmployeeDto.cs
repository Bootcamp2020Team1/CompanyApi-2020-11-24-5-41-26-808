using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyApi.Model
{
    public class EmployeeDto 
    {
        public EmployeeDto(string name, double salary)
        {
            Name = name;
            Salary = salary;
        }

        public EmployeeDto()
        {
        }

        public string Name { get; set; }
        public double? Salary { get; set; }
    }
}
