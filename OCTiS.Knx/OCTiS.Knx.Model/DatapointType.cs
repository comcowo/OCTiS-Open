using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Knx.Infrastructure.DataTypes;

namespace OCTiS.Knx.Model
{
    public class DatapointType
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }
        public string Text { get; set; }
        public short SizeInBits { get; set; }
        public bool Signed
        {
            get
            {
                string text = Text + Name;
                return text.Contains("signed") && !text.Contains("unsigned");
            }
        }

        public InternalTypes Type
        {
            get
            {
                if (SizeInBits == 1)
                    return InternalTypes.Bool;
                if (Text.Contains("float"))
                    return InternalTypes.Float;
                if (!Signed)
                    return InternalTypes.Uint;
                return InternalTypes.Int;
            }
        }

        public int Max
        {
            get
            {
                if (Signed)
                    return (1 << (SizeInBits - 1)) - 1;
                else
                    return 1 << SizeInBits - 1;
            }
        }

        public int Min
        {
            get
            {
                if (Signed)
                    return -((1 << (SizeInBits - 1)));
                else
                    return 0;
            }
        }

        private IEnumerable<DatapointSubType> _subTypes;
        public IEnumerable<DatapointSubType> SubTypes
        {
            get { return _subTypes; }
            set
            {
                if (_subTypes != null)
                    foreach (var st in _subTypes)
                    {
                        st.Type = null;
                    }
                _subTypes = value;
                if (_subTypes != null)
                    foreach (var st in _subTypes)
                    {
                        st.Type = this;
                    }
            }
        }
    }
}
