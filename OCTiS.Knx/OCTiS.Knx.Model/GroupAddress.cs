using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Knx.Infrastructure.DataTypes;

namespace OCTiS.Knx.Model
{
    public class GroupAddress
    {
        public string Id { get; set; }
        public GroupRange ParentRange { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public int Value { get; set; }

        protected List<GroupAddressInstance> _instances;
        public List<GroupAddressInstance> Instances
        {
            get
            {
                return _instances;
            }
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

        public IEnumerable<ComObjectInstance> ComObjectInstances
        {
            get
            {
                return Instances.Select(row => row.ComObjectInstance);
            }
        }

        public override string ToString()
        {
            return (new EnmxAddress(Value)).Address;
        }
    }
}