using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OCTiS.Knx.Model.Meta
{
    public class KnxMetaData
    {
        private Dictionary<string, string> _store;
        public KnxMetaData()
        {
            _store = new Dictionary<string, string>();
        }

        public IEnumerable<string> Keys
        {
            get { return _store.Keys; }
        }

        public string GetJSON()
        {
            return "{ " + string.Join(", ", _store.Keys.Select(row => "\"" + row + "\":\"" + _store[row] + "\"")) + " }";
        }

        public string this[string name]
        {
            get
            {
                if (_store.ContainsKey(name))
                    return _store[name];
                else
                    return null;
            }
            set
            {
                _store[name] = value;
            }
        }

        public string Name
        {
            get
            {
                return this["Name"];
            }
            set
            {
                this["Name"] = value;
            }
        }

        public string Description
        {
            get
            {
                return this["Description"];
            }
            set
            {
                this["Description"] = value;
            }
        }
    }
}
