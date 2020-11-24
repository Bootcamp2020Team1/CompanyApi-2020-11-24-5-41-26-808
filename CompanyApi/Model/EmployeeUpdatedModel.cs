using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyApi.Model
{
    public class EmployeeUpdatedModel 
    {
        public EmployeeUpdatedModel(string name, double salary)
        {
            Name = name;
            Salary = salary;
        }

        public EmployeeUpdatedModel()
        {
        }

        public string EmployeeID { get; set; }
        public string Name { get; set; }
        public double Salary { get; set; }
    }
}
