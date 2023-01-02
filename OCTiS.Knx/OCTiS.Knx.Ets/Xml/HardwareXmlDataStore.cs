using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace OCTiS.Knx.Ets.Xml
{
    public class HardwareXmlDataStore : XmlDataStore
    {
        public HardwareXmlDataStore(Stream s, string fileName)
            : base(s, fileName)
        {

        }
    }
}
