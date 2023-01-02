using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OCTiS.Knx.Model
{
    public class GroupRange
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int RangeStart { get; set; }
        public int RangeEnd { get; set; }

        private List<GroupAddress> _addresses;
        public List<GroupAddress> Addresses
        {
            get { return _addresses; }
            set 
            {
                if (_addresses != null)
                {
                    foreach (var addr in _addresses)
                    {
                        addr.ParentRange = null;
                    }
                }
                _addresses = value; 
                if (_addresses != null)
                {
                    foreach (var addr in _addresses)
                    {
                        addr.ParentRange = this;
                    }
                }
            }
        }


        public GroupRange ParentRange { get; set; }
        private List<GroupRange> _ranges;

        public List<GroupRange> Ranges
        {
            get { return _ranges; }
            set 
            {
                if (_ranges != null)
                {
                    foreach (var range in _ranges)
                    {
                        range.ParentRange = null;
                    }
                }
                _ranges = value; 
                if (_ranges != null)
                {
                    foreach (var range in _ranges)
                    {
                        range.ParentRange = this;
                    }
                }
            }
        }
        

        public IEnumerable<object> Children
        {
            get
            {
                if (Addresses == null)
                    return Ranges;
                if (Ranges == null)
                    return Addresses;
                return Addresses.AsEnumerable<object>().Union(Ranges.AsEnumerable<object>());
            }
        }
    }
}
