using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace OCTiS.Knx.Model
{
    public sealed class Installation
    {
        public string Name { get; set; }

        private List<Area> _areas;
        public List<Area> Areas 
        {
            get { return _areas; }
            set
            {
                if (_areas != null && _areas != value)
                {
                    foreach (var area in _areas)
                        area.Installation = null;
                }
                if (value != null)
                {
                    foreach (var area in value)
                        area.Installation = this;
                }
                _areas = value;
            }
        }
        private List<Space> _spaces;
        public List<Space> Spaces
        {
            get { return _spaces; }
            set
            {
                if (_spaces != null && _spaces != value)
                {
                    foreach (var space in _spaces)
                        space.Installation = null;
                }
                if (value != null)
                {
                    foreach (var space in value)
                        space.Installation = this;
                }
                _spaces = value;
            }
        }
    }
}
