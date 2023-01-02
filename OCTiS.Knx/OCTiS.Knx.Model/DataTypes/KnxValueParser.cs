using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Knx.Infrastructure.DataTypes
{
    public class KnxValueParser
    {
        public static PROPTYPE Parse<PROPTYPE>(object value)
        {
            if (value is byte[])
            {
                byte[] data = (byte[])value;
                if (typeof(PROPTYPE) == typeof(int))
                    return (PROPTYPE)ParseAsInt32(data);
                if (typeof(PROPTYPE) == typeof(bool))
                    return (PROPTYPE)ParseAsBool(data);
            }
            if (typeof(PROPTYPE) == value.GetType())
                return (PROPTYPE)value;
            throw new ArgumentException("Unknown KnxValue.");
        }

        private static object ParseAsInt32(byte[] data)
        {
            if (data.Length <= 4)
            {
                byte[] idata = new byte[4 - data.Length];
                idata = data.Concat(idata).ToArray();
                return BitConverter.ToInt32(idata, 0);
            }
            throw new ArgumentException("KnxValue is not int32.");
        }

        private static object ParseAsBool(byte[] data)
        {
            if (data.Length == 1)
                return Convert.ToBoolean(data[0]);
            throw new ArgumentException("KnxValue is not bool.");
        }
    }
}
