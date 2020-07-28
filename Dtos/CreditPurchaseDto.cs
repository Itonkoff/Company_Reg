using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Company_Reg.Dtos
{
    public class CreditPurchaseDto
    {
        public string UserID { get; set; }
        public string Service { get; set; }
        public int NumberOfCredits { get; set; }
    }
}
