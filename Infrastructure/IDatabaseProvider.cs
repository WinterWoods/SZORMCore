using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using SZORM.DbExpressions;
using SZORM.Factory;

namespace SZORM.Infrastructure
{
    public interface IDatabaseProvider
    {
        string DatabaseType { get; }
        IDbConnection CreateConnection(string connStr);
        IDbExpressionTranslator CreateDbExpressionTranslator();
        IStructure CreateStructureCheck();
        string CreateParameterName(string name);
        DbExpressionVisitor<DbExpression> GetSqlGenerator();
    }
}
