using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SZORM.Factory.SqlServer;
using SZORM.DbExpressions;
namespace SZORM.SqlServer.MethodHandlers
{
    class AddMinutes_Handler : IMethodHandler
    {
        public bool CanProcess(DbMethodCallExpression exp)
        {
            if (exp.Method.DeclaringType != UtilConstants.TypeOfDateTime)
                return false;

            return true;
        }
        public void Process(DbMethodCallExpression exp, SqlGenerator generator)
        {
            SqlGenerator.DbFunction_DATEADD(generator, "MINUTE", exp);
        }
    }
}
