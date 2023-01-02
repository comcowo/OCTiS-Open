using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OCTiS.Knx.Model
{
    public class Programming
    {
        public List<ApplicationProgram> Programs { get; set; }

        public IEnumerable<ComObject> ComObjects
        {
            get
            {
                return Programs.SelectMany(row => row.ComObjects);
            }
        }
    }
}
