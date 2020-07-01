using LinqToDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Company_Reg.Models;

namespace Company_Reg.Database
{
    public class db : LinqToDB.Data.DataConnection
    {

        public db() : base(Database.sql_server_db.configuration) { }

        public ITable<mSearchInfo> SearchInfo => GetTable<mSearchInfo>();
        public ITable<mSearchNames> SearchDetails => GetTable<mSearchNames>();
        public ITable<mCompanyInfo> CompanyInfo => GetTable<mCompanyInfo>();
        public ITable<mDirectorInfo> DirectorInfo => GetTable<mDirectorInfo>();
        public ITable<mMembersInfo> MembersInfo => GetTable<mMembersInfo>();
        public ITable<mDirectorsPotifolio> DirectorsPortifolio => GetTable<mDirectorsPotifolio>();
        public ITable<mApplicationReferences> ApplicationRef => GetTable<mApplicationReferences>();

        public ITable<mCompaniesReferences> CompaniesRef => GetTable<mCompaniesReferences>();

        public ITable<mApplications> applications => GetTable<mApplications>();
        public ITable<mTasks> taskss => GetTable<mTasks>();
        public ITable<mMembersPotifolio> MembersPortifolio => GetTable<mMembersPotifolio>();

        public ITable<mmainClause> objects => GetTable<mmainClause>();
        public ITable<mArticles> articles => GetTable<mArticles>();
        public ITable<mMemorandum> memo => GetTable<mMemorandum>();

       


    }

}
