using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using SZORM.Infrastructure;
using MySql.Data.MySqlClient;
using SZORM.DbExpressions;

namespace SZORM.Factory.MySql
{
    class MySqlDatabaseProvider : IDatabaseProvider
    {
        IDbConnection _dbDbConnection;

        public string DatabaseType { get { return "MySql"; } }

        public IDbConnection CreateConnection(string connStr)
        {
            return this._dbDbConnection=new MySqlConnection(connStr);
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
