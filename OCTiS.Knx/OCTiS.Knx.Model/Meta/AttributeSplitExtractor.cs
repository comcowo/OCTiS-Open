using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OCTiS.Knx.Model.Meta
{
    public class AttributeSplitExtractor : AttributeExtractor
    {
        public string Mark { get; set; }
        public bool UntilMark { get; set; }
        public override void Extract(Filtering.FilteredResult fr, ref KnxPort meta)
        {
            base.Extract(fr, ref meta);
            var result = meta.MetaData[AttributeName];
            if (!string.IsNullOrWhiteSpace(result))
            {
                var idx = result.IndexOf(Mark, StringComparison.InvariantCultureIgnoreCase);
                if (idx >= 0)
                {
                    if (UntilMark)
                        meta.MetaData[AttributeName] = result.Substring(0, idx);
                    else if (result.Length > idx + Mark.Length)
                        meta.MetaData[AttributeName] = result.Substring(idx + Mark.Length);
                }
            }
        }
    }
}
