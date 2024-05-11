namespace KerbalSimpit.Debugger.Utilities
{
    public static class DebugHelper
    {
        public static bool Ratio = false;

        public static string Get(short input)
        {
            if (Ratio)
            {
                return ((float)input / (float)short.MaxValue).ToString("0.000");
            }

            return input.ToString();
        }

        public static string Get(int input)
        {
            if (Ratio)
            {
                return ((float)input / (float)int.MaxValue).ToString("0.000");
            }

            return input.ToString("#,###,##0");
        }

        public static string Get(float input)
        {
            if (Ratio)
            {
                return ((float)input / (float)float.MaxValue).ToString("0.000");
            }

            return input.ToString("#,###,##0.000");
        }
    }
}
