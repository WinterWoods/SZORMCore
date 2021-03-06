﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using SZORM.Infrastructure;
using System.Data.SqlClient;
using SZORM.DbExpressions;

namespace SZORM.Factory.SqlServer
{
    class SqlServerDatabaseProvider : IDatabaseProvider
    {
        IDbConnection _dbDbConnection;

        public string DatabaseType { get { return "SqlServer"; } }

        public IDbConnection CreateConnection(string connStr)
        {
            return this._dbDbConnection = new SqlConnection(connStr);
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
            return new SqlServer.StructureToSqlServer();
        }
        public DbExpressionVisitor<DbExpression> GetSqlGenerator()
        {
            return SqlGenerator.CreateInstance();
        }
    }
}
