using Company_Reg.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Company_Reg.Dtos
{
    public class NameAddressResponceDto
    {
        public string Name{ get; set; }
        public string Type { get; set; }
        public string Justification { get; set; }

        public RegisteredOffice Office{ get; set; }
    }
}
