using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OCTiS.Knx.Model
{
    public class ComObjectRef
    {
       public ComObjectRef()
        {
            _values = new Dictionary<string, InstancedValue>();
        }

        public string Id { get; set; }

        public string RefId { get; set; }

        private ComObject _ref;
        public ComObject Ref
        {
            get { return _ref; }
            set
            {
                if (value != null)
                {
                    foreach (var attr in value.Attributes)
                    {
                        if (!_values.ContainsKey(attr.Key))
                        {
                            string name = attr.Key;
                            _values[name] = new InstancedValue(() => Ref[name]);
                        }
                    }
                }
                _ref = value;
            }
        }

        private List<ComObjectInstance> _instances;
        public List<ComObjectInstance> Instances
        {
            get { return _instances; }
            set
            {
                if (_instances != null)
                {
                    foreach (var instance in _instances)
                        instance.Ref = null;
                }
                _instances = value;
                if (_instances != null)
                {
                    foreach (var instance in _instances)
                        instance.Ref = this;
                }
            }
        }

        protected Dictionary<string, InstancedValue> _values;
        public InstancedValue this[string name]
        {
            get
            {
                if (!_values.ContainsKey(name))
                    _values[name] = new InstancedValue(() => Ref[name]);
                return _values[name];
            }
            set
            {
                if (!_values.ContainsKey(name))
                    _values[name] = new InstancedValue(() => Ref[name]);
                _values[name].Value = value;
            }
        }

        public ApplicationProgram Program { get; set; }

        public Dictionary<string, InstancedValue> Attributes
        {
            get { return _values; }
        }
    }
}
