using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OCTiS.Knx.Model
{
    public class InstancedValue
    {
        protected bool _overridden = false;

        public bool Overridden
        {
            get { return _overridden; }
            set { _overridden = value; }
        }

        private Func<object> _reference;
        public InstancedValue(Func<object> reference)
        {
            _reference = reference;
        }

        private object _value;

        public object Value
        {
            get
            {
                if (Overridden)
                    return _value;
                return _reference();
            }
            set
            {
                _value = value;
                _overridden = true;
            }
        }

        public override string ToString()
        {
            if (Value == null)
                return null;
            return Value.ToString();
        }
    }
}
