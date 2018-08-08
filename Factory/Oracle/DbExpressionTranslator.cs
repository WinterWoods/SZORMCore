﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SZORM.DbExpressions;
using SZORM.Infrastructure;

namespace SZORM.Factory.Oracle
{
    class DbExpressionTranslator : IDbExpressionTranslator
    {
        public static readonly DbExpressionTranslator Instance = new DbExpressionTranslator();

        public string Translate(DbExpression expression, out List<DbParam> parameters)
        {
            SqlGenerator generator = SqlGenerator.CreateInstance();
            expression.Accept(generator);

            parameters = generator.Parameters;
            string sql = generator.SqlBuilder.ToSql();

            return sql;
        }
    }
}
