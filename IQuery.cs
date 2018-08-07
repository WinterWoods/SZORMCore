using System;
using System.Collections.Generic;
using System.Text;

namespace SZORM
{
    public interface IQuery
    {
        Type ElementType { get; }
    }
}
