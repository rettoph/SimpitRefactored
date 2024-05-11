using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KerbalSimpit.Core.Utilities
{
    internal static class ThrowIf
    {
        public static class Type
        {
            public static void IsNotAssignableTo<TTo>(System.Type from)
            {
                if(typeof(TTo).IsAssignableFrom(from) == false)
                {
                    throw new ArgumentException($"Type {from.Name} is not assignable to {typeof(TTo).Name}.");
                }
            }
        }
    }
}
