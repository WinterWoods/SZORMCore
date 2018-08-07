using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Microsoft.Data.Sqlite;
using SZORM.DbExpressions;
using SZORM.Infrastructure;

namespace SZORM.Factory.SQLite
{
    class SqliteDatabaseProvider : IDatabaseProvider
    {
        IDbConnection _dbDbConnection;

        public string DatabaseType { get { return "Sqlite"; } }

        public IDbConnection CreateConnection(string connStr)
        {
            return this._dbDbConnection = new SqliteConnection(connStr);
        }
        public IDbExpressionTranslator CreateDbExpressionTranslator()
        {
            return DbExpressionTranslator.Instance;
        }
        public string CreateParameterName(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            if (name[0] == UtilConstants.ParameterNamePlaceholer[0])
            {
                return name;
            }

            return UtilConstants.ParameterNamePlaceholer + name;
        }

        public IStructure CreateStructureCheck()
        {
            return new MySql.StructureToMySql();
        }

        public DbExpressionVisitor<DbExpression> GetSqlGenerator()
        {
            return SqlGenerator.CreateInstance();
        }
    }
}
