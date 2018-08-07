using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using SZORM.Core.Visitors;
using SZORM.DbExpressions;
using SZORM.Utility;

namespace SZORM.Query.Visitors
{
    class FilterPredicateExpressionVisitor : ExpressionVisitor<DbExpression>
    {
        public static DbExpression ParseFilterPredicate(LambdaExpression lambda, ScopeParameterDictionary scopeParameters, KeyDictionary<string> scopeTables)
        {
            return GeneralExpressionVisitor.ParseLambda(ExpressionVisitorBase.ReBuildFilterPredicate(lambda), scopeParameters, scopeTables);
        }
    }
}
