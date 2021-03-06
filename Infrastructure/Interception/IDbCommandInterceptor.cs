﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SZORM.Infrastructure.Interception
{
    public interface IDbCommandInterceptor
    {
        void ReaderExecuting(IDbCommand command, DbCommandInterceptionContext<IDataReader> interceptionContext);
        void NonQueryExecuting();
        void ReaderExecuted(IDbCommand command, DbCommandInterceptionContext<IDataReader> interceptionContext);

        void NonQueryExecuting(IDbCommand command, DbCommandInterceptionContext<int> interceptionContext);
        void NonQueryExecuted(IDbCommand command, DbCommandInterceptionContext<int> interceptionContext);

        void ScalarExecuting(IDbCommand command, DbCommandInterceptionContext<object> interceptionContext);
        void ScalarExecuted(IDbCommand command, DbCommandInterceptionContext<object> interceptionContext);
    }
}
