using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Company_Reg.Models
{
    public class mTasks
    {

     [PrimaryKey]

     public  string _id { get; set; }
     public string Service { get; set; }

     public string Assigner { get; set; }

     public string AssignTo { get; set; }

     public string Date { get; set; }

     public string Status { get; set; }
        public string ExpDateofComp { get; set; }

        public string ExpTimeofComp { get; set; }


        public int NoOfRecords { get; set; }
    }
}
