using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OCTiS.Knx.Model
{
    public class BuildingPart
    {
        public BuildingPart Parent { get; set; }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Comment { get; set; }
        public string Number { get; set; }
        public string Type { get; set; }

        private List<BuildingPart> _parts;
        public List<BuildingPart> Parts
        {
            get { return _parts; }
            set
            {
                if (_parts != null)
                {
                    foreach (var part in _parts)
                    {
                        part.Parent = null;
                    }
                }
                _parts = value;
                if (_parts != null)
                {
                    foreach (var part in _parts)
                    {
                        part.Parent = this;
                    }
                }
            }
        }
        private List<DeviceInstance> _devices;
        public List<DeviceInstance> Devices
        {
            get { return _devices; }
            set 
            {
                if (_devices != null)
                {
                    foreach (var dev in _devices)
                        dev.Building = null;
                }
                _devices = value; 
                if (_devices != null)
                {
                    foreach (var dev in _devices)
                        dev.Building = this;
                }
            }
        }


        public IEnumerable<object> Children
        {
            get 
            {
                if (Parts == null)
                    return Devices;
                if (Devices == null)
                    return Parts;
                return Parts.AsEnumerable<object>().Union(Devices.AsEnumerable<object>()); 
            }
        }
    }
}
