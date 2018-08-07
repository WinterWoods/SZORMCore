using System;
using System.Collections.Generic;
using System.Text;
using SZORM.DbExpressions;

namespace SZORM.Query.QueryState
{
    class DistinctQueryState : SubQueryState
    {
        public DistinctQueryState(ResultElement resultElement)
            : base(resultElement)
        {
        }

        public override DbSqlQueryExpression CreateSqlQuery()
        {
            DbSqlQueryExpression sqlQuery = base.CreateSqlQuery();
            sqlQuery.IsDistinct = true;

            return sqlQuery;
        }
    }
}
