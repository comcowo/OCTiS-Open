using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OCTiS.Knx.Model
{
    public class ComObjectInstance
    {
        public ComObjectInstance()
        {
            _values = new Dictionary<string, InstancedValue>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public DatapointSubType DatapointType { get; set; }

        public DeviceInstance ParentDevice { get; set; }
        public Space ParentSpace { get; set; }

        private List<GroupAddressInstance> _groupAddresses;
        public List<GroupAddressInstance> GroupAddresses
        {
            get { return _groupAddresses; }
            set
            {
                if (_groupAddresses != null)
                {
                    foreach (var ga in _groupAddresses)
                        ga.ComObjectInstance = null;
                }
                _groupAddresses = value;
                if (_groupAddresses != null)
                {
                    foreach (var ga in _groupAddresses)
                        ga.ComObjectInstance = this;
                }
            }
        }

        public string Addresses
        {
            get
            {
                return string.Join(", ", _groupAddresses.Select(row => row.Ref.ToString()));
            }
        }

        private ComObjectRef _ref;
        public ComObjectRef Ref
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
                            _values[name] = new InstancedValue(() => getValue(name));
                        }
                    }
                }
                _ref = value;
            }
        }
        public ApplicationProgram Program { get; set; }

        protected Dictionary<string, InstancedValue> _values;
        public InstancedValue this[string name]
        {
            get
            {
                if (!_values.ContainsKey(name))
                    _values[name] = new InstancedValue(() => getValue(name));
                return _values[name];
            }
            set
            {
                if (!_values.ContainsKey(name))
                    _values[name] = new InstancedValue(() => getValue(name));
                _values[name].Value = value;
            }
        }

        private object getValue(string name)
        {
            return Ref != null ? Ref[name] : null;
        }

        public IEnumerable<InstanceAttribute> Attributes
        {
            get
            {
                return _values.Select(row => new InstanceAttribute() { Name = row.Key, Value = row.Value.Value, Overridden = row.Value.Overridden });
            }
        }

        public short Bits
        {
            get
            {
                return Parser.ObjectSize.ToBits(this["ObjectSize"].ToString());
            }
        }
    }

    public class InstanceAttribute
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public bool Overridden { get; set; }
    }
}
