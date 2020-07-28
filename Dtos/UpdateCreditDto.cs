using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Company_Reg.Dtos
{
    public class UpdateCreditDto
    {
        public string UserId { get; set; }
        public string Service { get; set; }
        public string ApplicationRef { get; set; }
    }
}
