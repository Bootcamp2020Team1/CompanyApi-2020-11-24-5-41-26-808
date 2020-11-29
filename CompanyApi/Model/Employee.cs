using System;

namespace CompanyApi.Model
{
    public class Employee : IEquatable<Employee>
    {
        public Employee(string companyID, string name, double salary)
        {
            EmployeeID = companyID;
            Name = name;
            Salary = salary;
        }

        public Employee()
        {
        }

        public string EmployeeID { get; set; } = Guid.NewGuid().ToString();

        public string Name { get; set; }
        public double Salary { get; set; }

        public bool Equals(Employee employee)
        {
            if (employee == null)
            {
                return false;
            }

            if (employee.GetType() != this.GetType())
            {
                return false;
            }

            return Name == employee.Name && EmployeeID == employee.EmployeeID && Salary == employee.Salary;
        }
    }
}
