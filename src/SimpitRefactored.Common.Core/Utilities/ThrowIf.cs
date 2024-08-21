using System;

namespace SimpitRefactored.Common.Core.Utilities
{
    public static class ThrowIf
    {
        public static class Type
        {
            public static void IsNotAssignableTo<TTo>(System.Type from)
            {
                if (typeof(TTo).IsAssignableFrom(from) == false)
                {
                    throw new ArgumentException($"Type {from.Name} is not assignable to {typeof(TTo).Name}.");
                }
            }
        }
    }
}
