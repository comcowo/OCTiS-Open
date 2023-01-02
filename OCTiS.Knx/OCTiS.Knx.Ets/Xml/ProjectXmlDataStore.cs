using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;

namespace OCTiS.Knx.Ets.Xml
{
    public class ProjectXmlDataStore : XmlDataStore
    {
        public ProjectXmlDataStore(Stream s, string fileName)
            : base(s, fileName)
        {

        }

        public IEnumerable<XElement> GroupAddresses
        {
            get { return Document.Descendants(ns + "GroupAddress"); }
        }

        public IEnumerable<XElement> Devices
        {
            get { return Document.Descendants(ns + "DeviceInstance"); }
        }
    }

    public static class ProjectXmlDataStoreExtensions
    {
        public static IEnumerable<XElement> GroupAddresses(this IEnumerable<ProjectXmlDataStore> projects)
        {
            return projects.SelectMany(row => row.GroupAddresses);
        }

        public static IEnumerable<XElement> Devices(this IEnumerable<ProjectXmlDataStore> projects)
        {
            return projects.SelectMany(row => row.Devices);
        }
    }
}
