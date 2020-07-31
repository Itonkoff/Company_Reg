using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Company_Reg.Dtos
{
    public class RegisteredPvtEntitySummaryDto
    {
        public string ApplicationId { get; set; }
        public string TypeOfEntity { get; set; }
        public string RegisteredName { get; set; }
        public string RegisteredNumber { get; set; }
        public string Designation { get; set; }
    }
}
