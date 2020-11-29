using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyApi.Models
{
    public class UpdateCompany
    {
        public UpdateCompany()
        {
        }

        public UpdateCompany(string name)
        {
            this.Name = name;
        }

        public string Name { get; set; }
        public override bool Equals(object obj)
        {
            UpdateCompany other = (UpdateCompany)obj;
            return Name == other.Name;
        }
    }
}
