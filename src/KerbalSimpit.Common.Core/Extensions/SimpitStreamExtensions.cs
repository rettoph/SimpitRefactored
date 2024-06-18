using KerbalSimpit.Common.Core.Utilities;

namespace KerbalSimpit.Common.Core.Extensions
{
    public static class SimpitStreamExtensions
    {
        public static void WriteCheckSum(this SimpitStream stream)
        {
            byte checksum = CheckSumHelper.CalculateCheckSum(stream);
            stream.Write(checksum);
        }
    }
}
