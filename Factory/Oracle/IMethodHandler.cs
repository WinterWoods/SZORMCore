﻿using System;
using System.Collections.Generic;
using System.Text;
using SZORM.DbExpressions;

namespace SZORM.Factory.Oracle
{
    interface IMethodHandler
    {
        bool CanProcess(DbMethodCallExpression exp);
        void Process(DbMethodCallExpression exp, SqlGenerator generator);
    }
}
