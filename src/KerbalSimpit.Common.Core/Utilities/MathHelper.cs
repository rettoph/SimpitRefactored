using System;

namespace KerbalSimpit.Common.Core.Utilities
{
    public static class MathHelper
    {
        public static int Min(params int[] args)
        {
            int result = args[0];

            for (int i = 1; i < args.Length; i++)
            {
                result = Math.Min(result, args[i]);
            }

            return result;
        }
    }
}
