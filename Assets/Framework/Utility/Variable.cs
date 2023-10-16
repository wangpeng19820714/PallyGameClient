using System;

namespace GameFramework
{
    public abstract class Variable
    {

        public abstract Type Type { get; }

        public Variable()
        {
        }

        public abstract object GetValue();

        public abstract void SetValue(object value);

        public abstract void Clear();
    }
}
