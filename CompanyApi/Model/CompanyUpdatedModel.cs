using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyApi.Model
{
    public class CompanyUpdatedModel
    {
        public CompanyUpdatedModel()
        {
        }

        public CompanyUpdatedModel(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}
