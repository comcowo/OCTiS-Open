using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OCTiS.Knx.Model.Meta
{
    public class ManualOverrideExtractor : AttributeExtractor
    {
        public string GroupAddress { get; set; }
        public string Value { get; set; }
        public override void Extract(Filtering.FilteredResult fr, ref KnxPort meta)
        {
            if (fr.GroupAddress.Ref.ToString() == GroupAddress)
                meta.MetaData[AttributeName] = Value;
        }
    }
}
