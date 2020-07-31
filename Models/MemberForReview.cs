using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Company_Reg.Models
{
    public class MemberForReview
    {
        public string MemberType { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhysicalAddress { get; set; }
        public string NationalId { get; set; }
        public string Nationality { get; set; }
        public int NumberOfShares{ get; set; }
        public List<string> Roles{ get; set; }
        public int HasQuery { get; set; }
        public string Comment { get; set; }

    }
}
