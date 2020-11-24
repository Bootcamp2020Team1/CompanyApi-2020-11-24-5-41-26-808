using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyApi.Models
{
    public class CompanyUpdateModel
    {
        public CompanyUpdateModel()
        {
        }

        public CompanyUpdateModel(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}
