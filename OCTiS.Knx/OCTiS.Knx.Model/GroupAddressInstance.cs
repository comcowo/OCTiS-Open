using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OCTiS.Knx.Model
{
    public class GroupAddressInstance
    {
        public string Id { get; set; }
        public GroupAddress Ref { get; set; }
        public ComObjectInstance ComObjectInstance { get; set; }

        public override string ToString()
        {
            return Ref.ToString();
        }

        public class Equality : IEqualityComparer<GroupAddressInstance>
        {
            public bool Equals(GroupAddressInstance x, GroupAddressInstance y)
            {
                if (x.Ref == null || y.Ref == null)
                    return false;
                return x.Ref.Value == y.Ref.Value;
            }

            public int GetHashCode(GroupAddressInstance obj)
            {
                return obj.Ref.Value.GetHashCode();
            }
        }

        public static Equality EqualityComparer = new Equality();
    }
}
