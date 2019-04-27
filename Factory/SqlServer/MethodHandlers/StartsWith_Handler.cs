using SZORM.DbExpressions;
using SZORM.Factory.SqlServer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SZORM.SqlServer.MethodHandlers
{
    class StartsWith_Handler : IMethodHandler
    {
        public bool CanProcess(DbMethodCallExpression exp)
        {
            if (exp.Method != UtilConstants.MethodInfo_String_StartsWith)
                return false;

            return true;
        }
        public void Process(DbMethodCallExpression exp, SqlGenerator generator)
        {
            exp.Object.Accept(generator);
            generator.SqlBuilder.Append(" LIKE ");
            exp.Arguments.First().Accept(generator);
            generator.SqlBuilder.Append(" + '%'");
        }
    }
}
