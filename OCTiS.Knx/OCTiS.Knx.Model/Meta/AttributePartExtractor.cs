using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OCTiS.Knx.Model.Meta
{
    public class AttributePartExtractor : AttributeExtractor
    {
        public int SkipWords { get; set; }
        public int TakeWords { get; set; }
        public override void Extract(Filtering.FilteredResult fr, ref KnxPort meta)
        {
            base.Extract(fr, ref meta);
            var result = meta.MetaData[AttributeName];
            if (!string.IsNullOrWhiteSpace(result))
            {
                var splits = result.Split(' ');
                if (splits.Length < SkipWords + TakeWords)
                    return;
                meta.MetaData[AttributeName] = string.Join(" ", splits.Skip(SkipWords).Take(TakeWords));
            }
        }
    }
}
