using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using OCTiS.Knx.Ets.Xml;
using OCTiS.Knx.Model;

namespace OCTiS.Knx.Ets
{
    public class ProjectStore : IEnumerable<XmlDataStore>
    {
        protected XNamespace ns;
        private List<XmlDataStore> _stores;

        #region convenience methods
        public IEnumerable<TYPE> GetChildStore<TYPE>() where TYPE : XmlDataStore
        {
            return _stores.Where(row => row is TYPE).Select(row => row as TYPE);
        }

        public IEnumerable<ProjectXmlDataStore> Projects
        {
            get { return GetChildStore<ProjectXmlDataStore>(); }
        }

        public IEnumerable<ProjectHeaderXmlDataStore> ProjectHeaders
        {
            get { return GetChildStore<ProjectHeaderXmlDataStore>(); }
        }

        public IEnumerable<MasterXmlDataStore> Masters
        {
            get { return GetChildStore<MasterXmlDataStore>(); }
        }

        public IEnumerable<ManufacturerXmlDataStore> Manufacturers
        {
            get { return GetChildStore<ManufacturerXmlDataStore>(); }
        }

        public IEnumerable<HardwareXmlDataStore> Hardwares
        {
            get { return GetChildStore<HardwareXmlDataStore>(); }
        }

        public IEnumerable<CatalogXmlDataStore> Catalogs
        {
            get { return GetChildStore<CatalogXmlDataStore>(); }
        }
        #endregion

        protected ProjectStore()
        {
            _stores = new List<XmlDataStore>();
            ns = null;
        }

        private XmlDataStore GetDataStore(Stream s, string fileName)
        {

            if (Regex.IsMatch(fileName, "0.xml"))
            {
                var ds = new ProjectXmlDataStore(s, fileName);
                if (ns == null)
                    ns = ds.Document.Root.Name.Namespace;
                return ds;
            }
            if (Regex.IsMatch(fileName, "Project.xml"))
                return new ProjectHeaderXmlDataStore(s, fileName);
            if (Regex.IsMatch(fileName, "knx_master.xml"))
            {
                var ds = new MasterXmlDataStore(s, fileName);
                ns = ds.Document.Root.Name.Namespace;
                return ds;
            }
            if (Regex.IsMatch(fileName, "Hardware.xml"))
                return new HardwareXmlDataStore(s, fileName);
            if (Regex.IsMatch(fileName, "Catalog.xml"))
                return new CatalogXmlDataStore(s, fileName);
            if (Regex.IsMatch(fileName, "M-"))
                return new ManufacturerXmlDataStore(s, fileName);
            return new XmlDataStore(s, fileName);
        }

        public static ProjectStore Load(string zipFile)
        {
            ProjectStore result = new ProjectStore();
            Unzipper.Unzip(zipFile, result.AddFile);
            return result;
        }

        public static ProjectStore Load(Stream stream)
        {
            ProjectStore result = new ProjectStore();
            Unzipper.Unzip(stream, result.AddFile);
            return result;
        }

        public void AddFile(Stream s, UnzippedDescriptor description)
        {
            if (description.Extension != ".xml")
                return;
            _stores.Add(GetDataStore(s, description.FileName));
        }

        #region IEnumerable
        public IEnumerator<XmlDataStore> GetEnumerator()
        {
            return _stores.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _stores.GetEnumerator();
        }
        #endregion

        #region {{VIEWS}}

        public class GroupWithDeviceObjects
        {
            public XElement Group { get; set; }
            public IEnumerable<XElement> DeviceObjects { get; set; }
        }

        public IEnumerable<GroupWithDeviceObjects> SelectGroupWithDeviceObjects
        {
            get
            {
                var result = from g in Projects.GroupAddresses()
                       join d in Projects.Devices().Descendants(ns + "Send") on g.Attribute("Id").Value equals d.Attribute("GroupAddressRefId").Value
                       group d by g into g2
                       select new GroupWithDeviceObjects()
                       {
                           Group = g2.Key,
                           DeviceObjects = g2
                       };
                return result;
            }
        }

        #endregion
    }
}
