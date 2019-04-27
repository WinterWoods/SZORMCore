using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SZORM.DbExpressions;

namespace SZORM.Factory.PostgreSQL.MethodHandlers
{
    class Subtract_Handler : IMethodHandler
    {
        public bool CanProcess(DbMethodCallExpression exp)
        {
            if (exp.Method != UtilConstants.MethodInfo_DateTime_Subtract_DateTime)
                return false;

            return true;
        }
        public void Process(DbMethodCallExpression exp, SqlGenerator generator)
        {
            DbSubtractExpression dbSubtract = new DbSubtractExpression(exp.Type, exp.Object, exp.Arguments[0]);
            dbSubtract.Accept(generator);
        }
    }
}
