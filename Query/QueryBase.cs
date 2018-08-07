using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SZORM.Query.QueryExpressions;

namespace SZORM.Query
{
    abstract class QueryBase
    {
        public abstract QueryExpression QueryExpression { get; }
        public abstract bool TrackEntity { get; }
    }
}
