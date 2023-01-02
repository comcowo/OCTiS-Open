using Knx.Infrastructure.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCTiS.Knx.HomeAssistant.Model
{
    public class Cover
    {
        public EnmxAddress move_long_address { get; set; }
        public EnmxAddress move_short_address { get; set; }
        public EnmxAddress stop_address { get; set; }
        public EnmxAddress position_address { get; set; }
        public EnmxAddress position_state_address { get; set; }
        public EnmxAddress angle_address { get; set; }
        public EnmxAddress angle_state_address { get; set; }
        public int travelling_time_down { get; set; }
        public int travelling_time_up { get; set; }
        public bool invert_updown { get; set; }
        public bool invert_position { get; set; }
        public bool invert_angle { get; set; }
        public string device_class { get; set; }
        public string entity_category { get; set; }
    }
}
