using KerbalSimpit.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KerbalSimpit.Core.Extensions
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
