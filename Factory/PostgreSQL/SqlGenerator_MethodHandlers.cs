﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using SZORM.DbExpressions;
using SZORM.Utility;

namespace SZORM.Factory.PostgreSQL
{
    partial class SqlGenerator : DbExpressionVisitor<DbExpression>
    {
        static Dictionary<string, IMethodHandler> GetMethodHandlers()
        {
            var methodHandlers = new Dictionary<string, IMethodHandler>();

            var methodHandlerTypes = Assembly.GetExecutingAssembly().GetTypes().Where(a => a.IsClass && !a.IsAbstract && typeof(IMethodHandler).IsAssignableFrom(a) && a.Name.EndsWith("_Handler") && a.GetConstructor(Type.EmptyTypes) != null);

            foreach (Type methodHandlerType in methodHandlerTypes)
            {
                string handleMethodName = methodHandlerType.Name.Substring(0, methodHandlerType.Name.Length - "_Handler".Length);
                methodHandlers.Add(handleMethodName, (IMethodHandler)Activator.CreateInstance(methodHandlerType));
            }

            var ret = Utils.Clone(methodHandlers);
            return ret;
        }
    }
}
