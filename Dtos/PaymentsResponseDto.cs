using Company_Reg.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Company_Reg.Dtos
{
    public class PaymentsResponseDto
    {
        public double AccountBalance { get; set; }
        public List<Payment> Payments { get; set; }
    }
}
