using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SZORM.Query.QueryState
{
    class GeneralQueryState : QueryStateBase, IQueryState
    {
        public GeneralQueryState(ResultElement resultElement)
            : base(resultElement)
        {
        }

        public override ResultElement ToFromQueryResult()
        {
            if (this.Result.Condition == null)
            {
                ResultElement result = new ResultElement(this.Result.ScopeParameters, this.Result.ScopeTables);
                result.FromTable = this.Result.FromTable;
                result.MappingObjectExpression = this.Result.MappingObjectExpression;
                return result;
            }

            return base.ToFromQueryResult();
        }

    }
}
