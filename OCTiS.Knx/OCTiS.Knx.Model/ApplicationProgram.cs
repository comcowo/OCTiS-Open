using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OCTiS.Knx.Model
{
    public class ApplicationProgram
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string VisibleDescription { get; set; }

        private List<ComObject> _comObjects;
        public List<ComObject> ComObjects
        {
            get { return _comObjects; }
            set
            {
                if (_comObjects != null)
                {
                    foreach (var obj in _comObjects)
                        obj.Program = null;
                }
                _comObjects = value;
                if (_comObjects != null)
                {
                    foreach (var obj in _comObjects)
                        obj.Program = this;
                }
            }
        }

        private List<ComObjectRef> _comObjectRefs;
        public List<ComObjectRef> ComObjectRefs
        {
            get { return _comObjectRefs; }
            set
            {
                if (_comObjectRefs != null)
                {
                    foreach (var obj in _comObjectRefs)
                        obj.Program = null;
                }
                _comObjectRefs = value;
                if (_comObjectRefs != null)
                {
                    foreach (var obj in _comObjectRefs)
                        obj.Program = this;
                }
            }
        }
    }
}
