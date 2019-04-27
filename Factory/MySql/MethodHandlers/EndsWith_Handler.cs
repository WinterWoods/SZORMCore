using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SZORM.DbExpressions;
using SZORM.Factory.MySql;

namespace SZORM.MySql.MethodHandlers
{
    class EndsWith_Handler : IMethodHandler
    {
        public bool CanProcess(DbMethodCallExpression exp)
        {
            if (exp.Method != UtilConstants.MethodInfo_String_EndsWith)
                return false;

            return true;
        }
        public void Process(DbMethodCallExpression exp, SqlGenerator generator)
        {
            exp.Object.Accept(generator);

            generator.SqlBuilder.Append(" LIKE ");
            generator.SqlBuilder.Append("CONCAT(");
            generator.SqlBuilder.Append("'%',");
            exp.Arguments.First().Accept(generator);
            generator.SqlBuilder.Append(")");
        }
    }

}
