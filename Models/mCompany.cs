using LinqToDB.DataProvider.SapHana;
using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Company_Reg.Models
{
    public class mCompanyInfo
    {
        [Required]
        public string RegNumber { get; set; }
        [Required]
        [PrimaryKey]
        public string Application_Ref { get; set; }

        [Required]
        public string Search_Ref { get; set; }
        [Required]
        public string Search_Name_ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public int No_Of_Directors { get; set; }
        [Required]
        public string Type { get; set; }

        public string Date_Of_Incoperation { get; set; }
        [Required]
        public string Date_Of_Application { get; set; }
        [Required]
        public string Status { get; set; }
        public string Approved_By { get; set; }
        [Required]
        public string Examiner { get; set; }

        public string Comments { get; set; }

        public string Co_Secretary { get; set; }
        [Required]
        public string Registered_Address { get; set; }

        public string City { get; set; }
        [Required]
        public string Country { get; set; }

        public double Athorised_Share { get; set; }
        public double Issued_Share { get; set; }

        public string Articles { get; set; }

        public string AppliedBy { get; set; }

        public int step { get; set; }

        public string Payment { get; set; }

    }

    public class PostMemo

    {
        public mMemorandum memo { get; set; }

        public int step { get; set; }
    }

    public class mMemorandum
    {
        [PrimaryKey]
        public string _id { get; set; }

        public string Application_Ref { get; set; }

        public string liabilityClause { get; set; }


        public string sharesClause { get; set; }

        public List<mmainClause> objects { get; set; } = new List<mmainClause>();

    }

    public class postArticles
    {
        public int step { get; set; }

        public mArticles Articles { get; set; }
    }
    public class mArticles
    {

        public string _id { get; set; }

        public string Application_Ref { get; set; }
        public string articles { get; set; }

        public string articles_type { get; set; }
    }
    public class mTest
    {

        public string _id { get; set; }

    }

    public class mmainClause
    {
        [PrimaryKey]
        public string _id { get; set; }
        public string obj_num { get; set; }
        public string memo_id { get; set; }

        public string objective { get; set; }
        public string objType { get; set; }
    }

    public class mDirectorsPotifolio
    {
        public string director_id { get; set; }

        [PrimaryKey]
        public string company_director_id { get; set; }
        public string Company_Reg { get; set; }
        public string Date_Of_Appointment { get; set; }
        public string Date_Of_Resignation { get; set; }

        public string Status { get; set; }

        public bool CompanySec { get; set; }
        public string Application_Ref { get; set; }

    }

    public class mMembersPotifolio
    {
        [PrimaryKey]
        public string company_member_id { get; set; }
        public string member_id { get; set; }
        
        public int number_of_shares { get; set; }
        public string Date_Of_Appointment { get; set; }
        public string Date_Of_Resignation { get; set; }
        public string Application_Ref { get; set; }

        public int IsMember { get; set; }

        public int IsDirector { get; set; }

        public int IsCoSec { get; set; }
    }

    public class PostMembers
    {
        public string _id { get; set; }
        public int step { get; set; }
        public List<mMembersInfo> members { get; set; }

        public List<mMembersPotifolio> membersPotifolio { get; set; }
    }
    public class mDirectorInfo
    {
        public string director_id { get; set; }
        [PrimaryKey]
        public string ID_No { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Surname { get; set; }
        public string Names { get; set; }
        public string Initials { get; set; }

    }

    public class mMembersInfo
    {
        public string member_id { get; set; }
        [PrimaryKey]
        public string ID_No { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Surname { get; set; }
        public string Names { get; set; }
        public string Initials { get; set; }
        public string memberType { get; set; }
        public string Nationality { get; set; }

    }

    public class mCompany
    {
        public mCompanyInfo CompanyInfo { get; set; }

        public mMemorandum memo { get; set; }

        public List<mMembersPotifolio> MembersPotifolios { get; set; }

       public mArticles articles { get; set; }
        public List<mMembersInfo> members { get; set; } = new List<mMembersInfo>();
        

    }
}
