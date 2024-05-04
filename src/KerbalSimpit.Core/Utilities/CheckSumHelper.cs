using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KerbalSimpit.Core.Utilities
{
    public static class CheckSumHelper
    {
        public static byte CalculateCheckSum(SimpitStream input)
        {
            byte checksum = input.Peek(0);
            for (int i = 1; i < input.Length; i++)
            {
                checksum ^= input.Peek(i);
            }

            return checksum;
        }

        public static bool ValidateCheckSum(SimpitStream data)
        {
            byte expected = data.Pop();
            byte calculated = CheckSumHelper.CalculateCheckSum(data);

            bool valid = calculated == expected;
            return valid;
        }
    }
}
