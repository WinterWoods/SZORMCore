using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SZORM.Query.QueryExpressions
{
    class RootQueryExpression : QueryExpression
    {
        string _explicitTable;
        public RootQueryExpression(Type elementType, string explicitTable)
            : base(QueryExpressionType.Root, elementType, null)
        {
            this._explicitTable = explicitTable;
        }
        public string ExplicitTable { get { return this._explicitTable; } }
        public override T Accept<T>(QueryExpressionVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }
    }
}
