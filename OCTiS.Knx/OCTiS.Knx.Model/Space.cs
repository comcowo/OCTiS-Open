using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCTiS.Knx.Model
{
    public class Space : ItemBase
    {
        public Installation Installation { get; set; }

        private List<ComObjectInstance> _comObjectInstances;
        public List<ComObjectInstance> ComObjectInstances
        {
            get { return _comObjectInstances; }
            set
            {
                if (_comObjectInstances != null)
                    foreach (var coi in _comObjectInstances)
                        coi.ParentSpace = null;
                _comObjectInstances = value;
                if (_comObjectInstances != null)
                    foreach (var coi in _comObjectInstances)
                        coi.ParentSpace = this;
            }
        }
    }
}
