using System;
using System.Collections.Generic;
using System.Text;

namespace SZORM.Utility
{
    public class ConstantWrapper<T>
    {
        public ConstantWrapper(T value)
        {
            this.Value = value;
        }
        public T Value { get; private set; }
    }
}
