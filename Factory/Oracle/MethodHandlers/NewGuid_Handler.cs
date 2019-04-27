using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SZORM.DbExpressions;
using SZORM.Factory.Oracle;

namespace SZORM.Oracle.MethodHandlers
{
    class NewGuid_Handler : IMethodHandler
    {
        public bool CanProcess(DbMethodCallExpression exp)
        {
            if (exp.Method != UtilConstants.MethodInfo_Guid_NewGuid)
                return false;

            return false;
        }
        public void Process(DbMethodCallExpression exp, SqlGenerator generator)
        {
            //返回的是一个长度为 16 的 byte[]
            generator.SqlBuilder.Append("SYS_GUID()");
        }
    }
}
