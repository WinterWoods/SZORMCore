using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using SZORM.DbExpressions;
using SZORM.Factory;
using SZORM.Infrastructure;

namespace SZORM.Factory.PostgreSQL
{
    public class PostgreSQLDatabaseProvider : IDatabaseProvider
    {
        IDbConnection _dbDbConnection;

        public string DatabaseType { get { return "PostgreSQL"; } }

        public IDbConnection CreateConnection(string connStr)
        {
            return this._dbDbConnection = new NpgsqlConnection(connStr);
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
            return new PostgreSQL.StructureToPostgreSQL();
        }

        public DbExpressionVisitor<DbExpression> GetSqlGenerator()
        {
            return SqlGenerator.CreateInstance();
        }
    }
}
