using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OCTiS.Knx.Model.Filtering;

namespace OCTiS.Knx.Model.Meta
{
    public abstract class MetaExtractor
    {
        protected MetaExtractor()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }
        public string AttributeName { get; set; }
        public abstract void Extract(FilteredResult fr, ref KnxPort meta);
    }

}
