using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace OCTiS.Knx.Model.Parser
{
    public class ObjectSize
    {
        public static short ToBits(string objectSize)
        {
            try
            {
                if (string.IsNullOrEmpty(objectSize))
                    return 0;
                var split = objectSize.Split(' ');
                short nr = short.Parse(split[0]);
                bool bytes = Regex.IsMatch(split[1], "byte");
                if (bytes)
                    return (short)(nr * 8);
                return nr;
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}
