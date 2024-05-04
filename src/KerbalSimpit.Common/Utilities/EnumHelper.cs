using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KerbalSimpit.Common.Utilities
{
    public static class EnumHelper
    {
        private static Dictionary<Type, Array> _values;

        public static T GetEnumFromByte<T>(byte value)
            where T : Enum
        {
            return GetValues<T>()[value];
        }

        private static T[] GetValues<T>()
            where T : Enum
        {
            if (_values.TryGetValue(typeof(T), out Array values))
            {
                return (T[])values;
            }

            values = Enum.GetValues(typeof(T));
            _values.Add(typeof(T), values);

            return (T[])values;
        }
    }
}
