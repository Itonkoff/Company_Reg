using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Drawing;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;

using LinqToDB.Configuration;

namespace Company_Reg.Database
{
    public class sql_server_db
    {

        private static string db_server = "localhost";
        public static string db_name = "company_reg";
        private static string db_user_name = "sa";
        private static string db_user_password = "password123";
        public static string configuration = "SqlServer";




        public class ConnectionStringSettings : IConnectionStringSettings
        {
            public string ConnectionString { get; set; }
            public string Name { get; set; }
            public string ProviderName { get; set; }
            public bool IsGlobal => false;
        }




        public class MySettings : ILinqToDBSettings
        {
            public IEnumerable<IDataProviderSettings> DataProviders => Enumerable.Empty<IDataProviderSettings>();

            public string DefaultConfiguration => configuration;
            public string DefaultDataProvider => configuration;

            public IEnumerable<IConnectionStringSettings> ConnectionStrings
            {
                get
                {
                    yield return
                        new ConnectionStringSettings
                        {
                            Name = configuration,
                            ProviderName = configuration,
                            //ConnectionString = @"Server=" + db_server + ";Database=" + db_name + ";Trusted_Connection=True;"
                            ConnectionString = @"Server=" + db_server + ";Database=" + db_name + ";User Id=" + db_user_name + ";Password=" + db_user_password + ";Enlist=False;"
                        };
                }
            }
        }

    }
}
