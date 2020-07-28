using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Company_Reg.Models
{
    public class Credit
    {
        public string UserId { get; set; }
        [PrimaryKey]
        public string CreditId { get; set; }
        public string PaymentId { get; set; }
        public string Service { get; set; }
        public string ExpiryDate { get; set; }
        public string ApplicationRef { get; set; }
    }
}
