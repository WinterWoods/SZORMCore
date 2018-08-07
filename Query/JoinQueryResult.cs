using System;
using System.Collections.Generic;
using System.Text;
using SZORM.DbExpressions;

namespace SZORM.Query
{
    public class JoinQueryResult
    {
        public IMappingObjectExpression MappingObjectExpression { get; set; }
        public DbJoinTableExpression JoinTable { get; set; }
    }
}
