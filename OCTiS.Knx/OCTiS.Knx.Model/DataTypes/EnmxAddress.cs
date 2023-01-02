using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Knx.Infrastructure.DataTypes
{
    public class EnmxAddress
    {
        private int _value;
        public int Value
        {
            get
            {
                return _value;
            }
        }

        public string Address
        {
            get
            {
                return string.Format("{0}/{1}/{2}"
                    , (_value & (15 << 11)) >> 11
                    , (_value & (7 << 8)) >> 8
                    , (_value & 255));
            }
        }

        public EnmxAddress(int value)
        {
            _value = value;
        }

        public static EnmxAddress Parse(string value)
        {
            var groups = value.Split('/').Select(row => int.Parse(row)).ToArray();
            if (groups.Count() != 3)
                throw new ArgumentException("Not valid Group address");
            if (groups[0] >= 16 || groups[0] < 0
                || groups[1] >= 8 || groups[1] < 0
                || groups[2] >= 256 || groups[2] < 0)
                throw new ArgumentOutOfRangeException("Proper range of group addresses is [0-15]/[0-7]/[0-255].");
            int val = 0;
            val = groups[0] << 11;
            val = (groups[1] << 8) | val;
            val = groups[2] | val;
            return new EnmxAddress(val);
        }

        public override string ToString()
        {
            return Address;
        }

        #region Operators
        public static implicit operator EnmxAddress(int i)
        {
            return new EnmxAddress(i);
        }

        public static implicit operator int(EnmxAddress a)
        {
            return a.Value;
        }

        public static implicit operator EnmxAddress(string s)
        {
            return Parse(s);
        }

        public static bool operator ==(EnmxAddress a, EnmxAddress b)
        {
            return a._value == b._value;
        }

        public static bool operator !=(EnmxAddress a, EnmxAddress b)
        {
            return a._value != b._value;
        }

        public override bool Equals(object obj)
        {
            if (obj is EnmxAddress)
                return _value == (obj as EnmxAddress)._value;
            return false;
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }
        #endregion
    }
}
