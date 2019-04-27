using System;
using System.Collections.Generic;
using System.Text;
using SZORM;
using SZORM.DbExpressions;
using SZORM.Infrastructure;

namespace SZORM.Factory.PostgreSQL
{
    class DbExpressionTranslator : IDbExpressionTranslator
    {
        public static readonly DbExpressionTranslator Instance = new DbExpressionTranslator();

        public DbExpressionVisitor<DbExpression> GetSqlGenerator()
        {
            return SqlGenerator.CreateInstance();
        }

        public string Translate(DbExpression expression, out List<SZORM.DbParam> parameters)
        {
            SqlGenerator generator = SqlGenerator.CreateInstance();
            expression.Accept(generator);

            parameters = generator.Parameters;
            string sql = generator.SqlBuilder.ToSql();

            return sql;
        }
    }
}
