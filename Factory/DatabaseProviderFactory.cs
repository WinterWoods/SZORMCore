using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SZORM.Infrastructure;
using SZORM.Utility;

namespace SZORM.Factory
{
    internal class DatabaseProviderFactory
    {
        public static IDatabaseProvider CreateDatabaseProvider(string DatabaseType,IDbConnection dbConnection=null)
        {
            if (DatabaseType == "MySql")
            {
                return new MySql.MySqlDatabaseProvider();
            }
            //else if (DatabaseType == "Oracle")
            //{
            //    return new Oracle.DbContextServiceProvider(new Oracle.OracleConnectionFactory(config));
            //}
            //else if (DatabaseType == "SQLite")
            //{
            //    return new SQLite.DbContextServiceProvider(new SQLite.SQLiteConnectionFactory(config));
            //}
            //else if (DatabaseType == "SqlServer")
            //{
            //    return new SqlServer.DbContextServiceProvider(new SqlServer.SqlServerConnectionFactory(config));
            //}
            else
            {
                throw new Exception("暂不支持的数据库");
            }
        }
    }
}
