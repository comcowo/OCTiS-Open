using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace OCTiS.Knx.Model
{
    public sealed class Line
    {
        public Area Area { get; set; }

        private List<DeviceInstance> _deviceInstances;
        public List<DeviceInstance> Devices
        {
            get { return _deviceInstances; }
            set
            {
                if (_deviceInstances != null && _deviceInstances != value)
                {
                    foreach (var item in _deviceInstances)
                        item.Line = null;
                }
                if (value != null)
                {
                    foreach (var item in value)
                        item.Line = this;
                }
                _deviceInstances = value;
            }
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public short Address { get; set; }
    }
}
