using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;

namespace OCTiS.Knx.Ets.Xml
{
    public class ManufacturerXmlDataStore : XmlDataStore
    {
        public ManufacturerXmlDataStore(Stream s, string fileName)
            : base(s, fileName)
        {

        }

        public IEnumerable<XElement> ParameterTypes
        {
            get { return Document.Descendants(ns + "ParameterType"); }
        }
    }

    public static class ManufacturerXmlDataStoreExtensions
    {
        public static IEnumerable<XElement> ParameterTypes(this IEnumerable<ManufacturerXmlDataStore> manufacturers)
        {
            return manufacturers.SelectMany(row => row.ParameterTypes);
        }
    }
}
