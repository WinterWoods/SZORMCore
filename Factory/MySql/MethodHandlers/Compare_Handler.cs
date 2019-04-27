using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using SZORM.DbExpressions;
using SZORM.Extensions;
using SZORM.Factory.MySql;
using SZORM.Utility;

namespace SZORM.MySql.MethodHandlers
{
    class Compare_Handler : IMethodHandler
    {
        public bool CanProcess(DbMethodCallExpression exp)
        {
            return exp.Method.DeclaringType == Factory.MySql.UtilConstants.TypeOfSql;
        }
        public void Process(DbMethodCallExpression exp, SqlGenerator generator)
        {
            DbExpression left = exp.Arguments[0];
            DbExpression right = exp.Arguments[2];

            CompareType compareType = (CompareType)exp.Arguments[1].Evaluate();

            DbExpression newExp = null;
            switch (compareType)
            {
                case CompareType.eq:
                    {
                        MethodInfo method_Sql_Equals = Factory.MySql.UtilConstants.MethodInfo_Sql_Equals.MakeGenericMethod(left.Type);

                        /* Sql.Equals(left, right) */
                        DbMethodCallExpression left_equals_right = DbExpression.MethodCall(null, method_Sql_Equals, new List<DbExpression>(2) { left, right });

                        newExp = left_equals_right;
                    }
                    break;
                case CompareType.neq:
                    {
                        MethodInfo method_Sql_NotEquals = Factory.MySql.UtilConstants.MethodInfo_Sql_NotEquals.MakeGenericMethod(left.Type);

                        /* Sql.NotEquals(left, right) */
                        DbMethodCallExpression left_not_equals_right = DbExpression.MethodCall(null, method_Sql_NotEquals, new List<DbExpression>(2) { left, right });

                        newExp = left_not_equals_right;
                    }
                    break;
                case CompareType.gt:
                    {
                        newExp = new DbGreaterThanExpression(left, right);
                    }
                    break;
                case CompareType.gte:
                    {
                        newExp = new DbGreaterThanOrEqualExpression(left, right);
                    }
                    break;
                case CompareType.lt:
                    {
                        newExp = new DbLessThanExpression(left, right);
                    }
                    break;
                case CompareType.lte:
                    {
                        newExp = new DbLessThanOrEqualExpression(left, right);
                    }
                    break;
                default:
                    throw new NotSupportedException("CompareType: " + compareType.ToString());
            }

            newExp.Accept(generator);
        }
    }
}
