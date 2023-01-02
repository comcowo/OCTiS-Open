using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCTiS.Knx.Model
{
    public class GroupAddressInfo
    {
        private GroupAddress _GroupAddress;
        public string GroupAddress { get; }
        public string SpacePath { get; }
        public string AddressA { get; }
        public string AddressB { get; }
        public string AddressC { get; }
        public string AddressPath { get; }
        public bool HasConnections { get; }
        public GroupAddressInfo(GroupAddress groupAddress)
        {
            _GroupAddress = groupAddress;
            GroupAddress = groupAddress.ToString();
            AddressA = $"{groupAddress.ParentRange.ParentRange.Name}";
            AddressB = $"{groupAddress.ParentRange.Name}";
            AddressC = $"{groupAddress.Name}";
            AddressPath = $"{AddressA} - {AddressB} - {AddressC}";
            var devices = groupAddress.Instances.Select(i => i.ComObjectInstance?.ParentDevice).Where(s => s != null).ToList();
            HasConnections = devices.Count > 0;
            if (!HasConnections) return;
            var spaces = groupAddress.Instances.Select(i => i.ComObjectInstance?.ParentSpace).Where(s => s != null).ToList();
            if (spaces.Count > 0)
            {
                while (spaces.Count > 2) spaces.RemoveAt(0);
                SpacePath = String.Join(" - ", spaces.Select(s => s.Name));
            }
        }
    }
}
