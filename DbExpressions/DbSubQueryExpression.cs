﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SZORM.Utility;

namespace SZORM.DbExpressions
{
    public class DbSubQueryExpression : DbExpression
    {
        DbSqlQueryExpression _sqlQuery;

        public DbSubQueryExpression(DbSqlQueryExpression sqlQuery)
            : base(DbExpressionType.SubQuery)
        {
            this._sqlQuery = sqlQuery;
        }

        public DbSqlQueryExpression SqlQuery { get { return this._sqlQuery; } }
        public override Type Type { get { return this.SqlQuery.Type; } }

        public override T Accept<T>(DbExpressionVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }

    }
}
