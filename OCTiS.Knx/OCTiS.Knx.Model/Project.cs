using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OCTiS.Knx.Model
{
    public class Project
    {
        public List<Installation> Installations { get; set; }

        public List<GroupRange> GroupRanges { get; set; }

        public List<BuildingPart> Buildings { get; set; }

        public IEnumerable<DeviceInstance> Devices
        {
            get
            {
                return Installations
                .SelectMany(row => row.Areas)
                .SelectMany(row => row.Lines)
                .SelectMany(row => row.Devices);
            }
        }
    }
}
