using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using SZORM.DbExpressions;
using SZORM.Extensions;

namespace SZORM.Factory.PostgreSQL.MethodHandlers
{
    class NotEquals_Handler : IMethodHandler
    {
        public bool CanProcess(DbMethodCallExpression exp)
        {
            MethodInfo method = exp.Method;
            if (method.DeclaringType != UtilConstants.TypeOfSql)
            {
                return false;
            }

            return true;
        }
        public void Process(DbMethodCallExpression exp, SqlGenerator generator)
        {
            MethodInfo method = exp.Method;

            DbExpression left = exp.Arguments[0];
            DbExpression right = exp.Arguments[1];

            left = DbExpressionExtension.StripInvalidConvert(left);
            right = DbExpressionExtension.StripInvalidConvert(right);

            //明确 left right 其中一边一定为 null
            if (DbExpressionExtension.AffirmExpressionRetValueIsNull(right))
            {
                left.Accept(generator);
                generator.SqlBuilder.Append(" IS NOT NULL");
                return;
            }

            if (DbExpressionExtension.AffirmExpressionRetValueIsNull(left))
            {
                right.Accept(generator);
                generator.SqlBuilder.Append(" IS NOT NULL");
                return;
            }

            SqlGenerator.AmendDbInfo(left, right);

            left.Accept(generator);
            generator.SqlBuilder.Append(" <> ");
            right.Accept(generator);
        }
    }
}
