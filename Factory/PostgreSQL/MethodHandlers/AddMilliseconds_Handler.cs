using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SZORM.DbExpressions;
using SZORM.Extensions;
using SZORM.Factory;

namespace SZORM.Factory.PostgreSQL.MethodHandlers
{
    class AddMilliseconds_Handler : IMethodHandler
    {
        public bool CanProcess(DbMethodCallExpression exp)
        {
            if (exp.Method.DeclaringType != UtilConstants.TypeOfDateTime)
                return false;

            return true;
        }

        public void Process(DbMethodCallExpression exp, SqlGenerator generator)
        {
            exp.Object.Accept(generator);
            generator.SqlBuilder.Append(" + interval ");
            generator.SqlBuilder.Append("'");

            var argExp = exp.Arguments[0];
            if (!argExp.IsEvaluable())
                throw UtilExceptions.NotSupportedMethod(exp.Method);

            var arg = argExp.Evaluate();
            generator.SqlBuilder.Append(arg.ToString());
            generator.SqlBuilder.Append(" ");
            generator.SqlBuilder.Append("milliseconds");
            generator.SqlBuilder.Append("'");
        }
    }
}
