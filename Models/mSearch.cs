using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Company_Reg.Models
{
    public class mSearchInfo
    {
        [Required]
        [PrimaryKey]
        public string search_ID { get; set; } = Guid.NewGuid().ToString();
        [Required]
        public string Searcher_ID { get; set; }
        [Required]

        public string Search_For { get; set; }
        [Required]
        public string Purpose { get; set; }
        [Required]
        public string SearchDate { get; set; }
        [Required]
        public string Satus { get; set; }

        public string Examiner { get; set; }
        public string ApprovedDate { get; set; }

        public string Cost { get; set; }

        public string ExamanerTaskID { get; set; }
        public string Justification { get; set; }

        public string SortingOffice { get; set; }
        public string Desigination { get; set; }
        public string SearchRef { get; set; }
        public string Payment { get; set; }


    }


    public class mSearchNames
    {
        [Required]
        [PrimaryKey]
        public string Name_ID { get; set; } = Guid.NewGuid().ToString();
        [Required]
        public string Name { get; set; }
        [Required]
        public string Status { get; set; }

        [Required]

        public string Search_ID { get; set; }
    }


    public class mSearch
    {
        [Required]
        public mSearchInfo searchInfo { get; set; }

        [Required]
        public List<mSearchNames> SearchNames { get; set; } = new List<mSearchNames>();
    }
}
