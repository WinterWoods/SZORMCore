using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using SZORM.Infrastructure;
using Oracle.ManagedDataAccess.Client;
using SZORM.DbExpressions;

namespace SZORM.Factory.Oracle
{
    class OracleDatabaseProvider : IDatabaseProvider
    {
        IDbConnection _dbDbConnection;

        public string DatabaseType { get { return "Oracle"; } }

        public IDbConnection CreateConnection(string connStr)
        {
            return this._dbDbConnection = new SZORMOracleConnection(new OracleConnection(connStr));
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
            return new Oracle.StructureToOracle();
        }

        public DbExpressionVisitor<DbExpression> GetSqlGenerator()
        {
            return SqlGenerator.CreateInstance();
        }
    }
}
