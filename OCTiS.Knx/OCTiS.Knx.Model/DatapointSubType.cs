using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OCTiS.Knx.Model
{
    public class DatapointSubType
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public string Number { get; set; }
        public bool Default { get; set; }

        public DatapointType Type { get; set; }
    }
}
