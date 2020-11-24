using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyApi
{
    public class UpdateModel
    {
        public UpdateModel()
        {
        }

        public UpdateModel(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}
