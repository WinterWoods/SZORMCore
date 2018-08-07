using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using SZORM.DbExpressions;
using SZORM.Query.Mapping;
using SZORM.Query.QueryExpressions;
using SZORM.Utility;

namespace SZORM.Query.QueryState
{
    interface IQueryState
    {
        MappingData GenerateMappingData();

        ResultElement ToFromQueryResult();
        JoinQueryResult ToJoinQueryResult(JoinType joinType, LambdaExpression conditionExpression, ScopeParameterDictionary scopeParameters, KeyDictionary<string> scopeTables, string tableAlias);

        IQueryState Accept(WhereExpression exp);
        IQueryState Accept(OrderExpression exp);
        IQueryState Accept(SelectExpression exp);
        IQueryState Accept(SkipExpression exp);
        IQueryState Accept(TakeExpression exp);
        IQueryState Accept(AggregateQueryExpression exp);
        IQueryState Accept(GroupingQueryExpression exp);
        IQueryState Accept(DistinctExpression exp);
    }
}
