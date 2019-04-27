using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SZORM.DbExpressions;
using SZORM.Factory.Oracle;

namespace SZORM.Oracle.MethodHandlers
{
    class Parse_Handler : IMethodHandler
    {
        public bool CanProcess(DbMethodCallExpression exp)
        {
            if (exp.Arguments.Count != 1)
                return false;

            DbExpression arg = exp.Arguments[0];
            if (arg.Type != UtilConstants.TypeOfString)
                return false;

            Type retType = exp.Method.ReturnType;
            if (exp.Method.DeclaringType != retType)
                return false;

            return true;
        }
        public void Process(DbMethodCallExpression exp, SqlGenerator generator)
        {
            DbExpression arg = exp.Arguments[0];
            DbExpression e = DbExpression.Convert(arg, exp.Method.ReturnType);
            if (exp.Method.ReturnType == UtilConstants.TypeOfBoolean)
            {
                e.Accept(generator);
                generator.SqlBuilder.Append(" = ");
                DbConstantExpression.True.Accept(generator);
            }
            else
                e.Accept(generator);
        }
    }
}
