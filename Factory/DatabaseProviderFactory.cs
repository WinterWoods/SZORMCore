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
        public static IDatabaseProvider CreateDatabaseProvider(string DatabaseType, IDatabaseProvider dbDatabaseProvider =null)
        {
            if (DatabaseType.ToLower() == "MySql".ToLower())
            {
                return new MySql.MySqlDatabaseProvider();
            }
            else if (DatabaseType.ToLower() == "Oracle".ToLower())
            {
                return new Oracle.OracleDatabaseProvider();
            }
            else if (DatabaseType.ToLower() == "Sqlite".ToLower())
            {
                return new SQLite.SqliteDatabaseProvider();
            }
            else if (DatabaseType.ToLower() == "SqlServer".ToLower())
            {
                return new SqlServer.SqlServerDatabaseProvider();
            }
            else
            {
                return dbDatabaseProvider;
            }
        }
    }
}
