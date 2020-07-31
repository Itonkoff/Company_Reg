using Company_Reg.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Company_Reg.Dtos
{
    public class ApplicationForReviewDto
    {
        public NameForReview name { get; set; }
        public RegisteredOffice office { get; set; }
        public List<MemberForReview> members { get; set; } = new List<MemberForReview>();
        public MemoForReview memo{ get; set; }
        public List<mArticles> articles { get; set; }
    }
}
