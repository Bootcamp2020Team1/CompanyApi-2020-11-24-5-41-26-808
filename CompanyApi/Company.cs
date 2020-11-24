using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyApi
{
    public class Company
    {
        public Company()
        {
        }

        public Company(string name)
        {
            Name = name;
        }

        public int CompanyId { get; set; }
        public string Name { get; set; }
    }
}
