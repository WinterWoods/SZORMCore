using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SZORM.Query.QueryExpressions
{
    class OrderExpression : QueryExpression
    {
        LambdaExpression _keySelector;
        public OrderExpression(Type elementType, QueryExpression prevExpression, QueryExpressionType expressionType, LambdaExpression keySelector)
            : base(expressionType, elementType, prevExpression)
        {
            this._keySelector = keySelector;
        }
        public LambdaExpression KeySelector
        {
            get { return this._keySelector; }
        }

        public override T Accept<T>(QueryExpressionVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }
    }
}
