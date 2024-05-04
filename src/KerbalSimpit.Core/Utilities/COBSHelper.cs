using KerbalSimpit.Core.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KerbalSimpit.Core.Utilities
{
    public static class COBSHelper
    {
        public static bool TryEncodeCOBS(SimpitStream input, SimpitStream output)
        {
            output.Clear();
            output.Write(0x69); // A meaningless placeholder. It will get overwritten.
            int lastZero = 0;
            byte distanceLastZero = 1;
            while (input.Length > 0)
            {
                byte placeholder = input.ReadByte();
                if (placeholder == 0)
                {
                    output.Write(distanceLastZero, lastZero);

                    output.Write(0x69); // A meaningless placeholder. It will get overwritten.
                    lastZero = output.Length - 1;

                    distanceLastZero = 1;
                }
                else
                {
                    output.Write(placeholder);
                    distanceLastZero++;
                }
            }

            output.Write(distanceLastZero, lastZero);
            output.Write(SpecialBytes.EndOfMessage);

            return true;
        }

        public static bool TryDecodeCOBS(SimpitStream input, SimpitStream output)
        {
            if (input.Length <= 4)
            { // Not enough data to have a packet type, a payload, a checksum and the additionnal byte of COBS encoding
                return false;
            }

            output.Clear();

            int nextZero = input.ReadByte();
            byte placeholder;
            while (input.Length > 0)
            {
                placeholder = input.ReadByte();

                if (placeholder == 0)
                {
                    return (input.Length == 0) && (nextZero == 1);
                }

                nextZero--;
                if (nextZero == 0)
                {
                    output.Write(0);
                    nextZero = placeholder;
                }
                else
                {
                    output.Write(placeholder);
                }
            }

            return false;
        }
    }
}
