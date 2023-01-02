using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Knx.Infrastructure.DataTypes;

namespace OCTiS.Knx.Model.Meta
{
    public enum KnxType
    {
        Bool,
        Int
    }

    public class KnxPort
    {
        public EnmxAddress GroupAddress { get; set; }

        public KnxMetaData MetaData { get; set; }

        public KnxType Type { get; set; }

        public int MaxValue { get; set; }
        public int MinValue { get; set; }
    }
}
