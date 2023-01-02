using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OCTiS.Knx.Model.Filtering;

namespace OCTiS.Knx.Model.Meta
{
    public class DemoMeta
    {
        public static void Extract(FilteredResult fr, ref KnxPort port)
        {
            port.MetaData["Room"] = "";
            try
            {
                var button = fr.ComObjects.FirstOrDefault(row => row.ParentDevice.Product.Text.Contains("Push-button"));
                port.MetaData["Room"] = button.ParentDevice.Building.Name;
            }
            catch (Exception)
            {
                port.MetaData["Room"] = fr.ComObjects.Where(row => row.ParentDevice != null && row.ParentDevice.Building != null).Select(row => row.ParentDevice.Building.Name).FirstOrDefault() ?? "";
            }

        }
    }
    public class MetaDataRules
    {
        public Guid Id { get; set; }
        public List<MetaExtractor> Extractors { get; set; }
        public MetaDataRules()
        {
            Id = Guid.NewGuid();
            Extractors = new List<MetaExtractor>();
        }

        public KnxPort Extract(FilteredResult fr)
        {
            var port = new KnxPort() 
            { 
                GroupAddress = fr.GroupAddress.Ref.Value,
                MetaData = ConstructMetaData(fr),
                Type = fr.ComObjects.Any(row => row.Bits == 1) ? KnxType.Bool : KnxType.Int,
                MaxValue = fr.ComObjects.Select(row => 1 << row.Bits - 1 ).Min(),
                MinValue = 0
            };
            foreach (var extractor in Extractors.Where(row => !(row is ManualOverrideExtractor)))
                extractor.Extract(fr, ref port);
            DemoMeta.Extract(fr, ref port);
            foreach (var extractor in Extractors.Where(row => (row is ManualOverrideExtractor)))
                extractor.Extract(fr, ref port);
            return port;
        }

        public List<KnxPort> Extract(IEnumerable<FilteredResult> list)
        {
            return list.Select(row => Extract(row)).ToList();
        }

        private KnxMetaData ConstructMetaData(FilteredResult result)
        {
            KnxMetaData meta = new KnxMetaData();
            return meta;
        }
    }
}
