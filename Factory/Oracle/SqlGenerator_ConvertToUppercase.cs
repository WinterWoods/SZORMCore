using System;
using System.Collections.Generic;
using System.Text;

namespace SZORM.Factory.Oracle
{
    class SqlGenerator_ConvertToUppercase : SqlGenerator
    {
        protected override void QuoteName(string name)
        {
            base.QuoteName(name.ToUpper());
        }
    }
}
