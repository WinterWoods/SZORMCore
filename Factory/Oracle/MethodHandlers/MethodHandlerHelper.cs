using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SZORM.DbExpressions;
using System.Reflection;
using SZORM.Extensions;
using SZORM.Factory.Oracle;

namespace SZORM.Oracle.MethodHandlers
{
    class MethodHandlerHelper
    {
        public static void EnsureTrimCharArgumentIsSpaces(DbExpression exp)
        {
            if (!exp.IsEvaluable())
                throw new NotSupportedException();

            var arg = exp.Evaluate();
            if (arg == null)
                throw new ArgumentNullException();

            var chars = arg as char[];
            if (chars.Length != 1 || chars[0] != ' ')
            {
                throw new NotSupportedException();
            }
        }

        public static string AppendNotSupportedDbFunctionsMsg(MethodInfo method, string insteadProperty)
        {
            return string.Format("'{0}' is not supported. Instead of using '{1}.{2}'.", Utility.Utils.ToMethodString(method), Utility.Utils.ToMethodString(UtilConstants.MethodInfo_DateTime_Subtract_DateTime), insteadProperty);
        }
    }
}
