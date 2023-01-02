using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace OCTiS.Knx.Model
{
    public sealed class Area
    {
        public Installation Installation { get; set; }

        private List<Line> _lines;
        public List<Line> Lines
        {
            get { return _lines; }
            set
            {
                if (_lines != null && _lines != value)
                {
                    foreach (var line in _lines)
                        line.Area = null;
                }
                if (value != null)
                {
                    foreach (var line in value)
                        line.Area = this;
                }
                _lines = value;
            }
        }

        public string Name { get; set; }
        public string Description { get; set; }

        public short Address { get; set; }
    }
}
