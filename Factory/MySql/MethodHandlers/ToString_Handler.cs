using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SZORM.DbExpressions;
using SZORM.Extensions;
using SZORM.Factory.MySql;

namespace SZORM.MySql.MethodHandlers
{
    class ToString_Handler : IMethodHandler
    {
        public bool CanProcess(DbMethodCallExpression exp)
        {
            if (exp.Arguments.Count != 0)
            {
                return false;
            }

            if (exp.Object.Type == UtilConstants.TypeOfString)
            {
                return true;
            }

            if (!SqlGenerator.NumericTypes.ContainsKey(exp.Object.Type.GetUnderlyingType()))
            {
                return false;
            }

            return true;
        }
        public void Process(DbMethodCallExpression exp, SqlGenerator generator)
        {
            if (exp.Object.Type == UtilConstants.TypeOfString)
            {
                exp.Object.Accept(generator);
                return;
            }

            DbConvertExpression c = DbExpression.Convert(exp.Object, UtilConstants.TypeOfString);
            c.Accept(generator);
        }
    }
}
