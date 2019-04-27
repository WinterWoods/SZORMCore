using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SZORM.DbExpressions;
using SZORM.Factory;
using SZORM.Factory.SQLite;

namespace SZORM.SQLite.MethodHandlers
{
    class Substring_Handler : IMethodHandler
    {
        public bool CanProcess(DbMethodCallExpression exp)
        {
            if (exp.Method != UtilConstants.MethodInfo_String_Substring_Int32 && exp.Method != UtilConstants.MethodInfo_String_Substring_Int32_Int32)
                return false;

            return true;
        }
        public void Process(DbMethodCallExpression exp, SqlGenerator generator)
        {
            generator.SqlBuilder.Append("SUBSTR(");
            exp.Object.Accept(generator);
            generator.SqlBuilder.Append(",");

            DbExpression arg1 = exp.Arguments[0];
            if (arg1.NodeType == DbExpressionType.Constant)
            {
                int startIndex = (int)(((DbConstantExpression)arg1).Value) + 1;
                generator.SqlBuilder.Append(startIndex.ToString());
            }
            else
            {
                arg1.Accept(generator);
                generator.SqlBuilder.Append(" + 1");
            }

            if (exp.Method == UtilConstants.MethodInfo_String_Substring_Int32)
            {

            }
            else if (exp.Method == UtilConstants.MethodInfo_String_Substring_Int32_Int32)
            {
                generator.SqlBuilder.Append(",");
                exp.Arguments[1].Accept(generator);
            }
            else
                throw UtilExceptions.NotSupportedMethod(exp.Method);

            generator.SqlBuilder.Append(")");
        }
    }
}
