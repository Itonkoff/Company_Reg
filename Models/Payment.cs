using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Company_Reg.Models
{
    public class Payment
    {
        public string UserId { get; set; }
        [PrimaryKey]
        public string PaymentId { get; set; }
        public string PaynowReference { get; set; }
        public string Date { get; set; }
        public string Description { get; set; }
        public double AmountDr { get; set; }
        public double AmountCr { get; set; }
    }
}
