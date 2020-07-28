using Company_Reg.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Company_Reg.Dtos
{
    public class CreditPurchaseResponseDto
    {
        public Payment payment { get; set; }
        public List<Credit> credits { get; set; }
    }
}
