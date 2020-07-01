using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Company_Reg.Models
{
    public class mApplications
    {

        [Required]
        [PrimaryKey]
        public string ApplicationID { get; set; }
       public string ServiceApplied { get; set; }
       public string DateOfApplication { get; set; }
       public string Status { get; set; }
       public int Payment { get; set; }
        public string AppliedBy { get; set; }
    }
}
