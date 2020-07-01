using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Company_Reg.Models
{
    public class mCompaniesReferences
    {
        [PrimaryKey]
        public string _id { get; set; }
        public string LastRegNo { get; set; }
      public string Year { get; set; }
      public string Prefix { get; set; }
      public string Status { get; set; }
    }

    public class mApplicationReferences
    {
        [PrimaryKey]
        public string _id { get; set; }
     public string LastApplicationRef { get; set; }
    public string Prefix { get; set; }
      public string Year { get; set; }
      public string Status { get; set; }
    }
}
