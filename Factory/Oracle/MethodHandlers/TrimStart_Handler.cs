using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SZORM.DbExpressions;
using SZORM.Factory.Oracle;

namespace SZORM.Oracle.MethodHandlers
{
    class TrimStart_Handler : IMethodHandler
    {
        public bool CanProcess(DbMethodCallExpression exp)
        {
            if (exp.Method != UtilConstants.MethodInfo_String_TrimStart)
                return false;

            return true;
        }
        public void Process(DbMethodCallExpression exp, SqlGenerator generator)
        {
            MethodHandlerHelper.EnsureTrimCharArgumentIsSpaces(exp.Arguments[0]);

            generator.SqlBuilder.Append("LTRIM(");
            exp.Object.Accept(generator);
            generator.SqlBuilder.Append(")");
        }
    }
}
