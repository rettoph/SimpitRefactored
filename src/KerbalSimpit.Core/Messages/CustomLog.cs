using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KerbalSimpit.Core.Messages
{
    public struct CustomLog : ISimpitMessageContent
    {
        [Flags]
        public enum FlagsEnum
        {
            Verbose = 1,
            PrintToScreen = 2,
            NoHeader = 4
        }

        public FlagsEnum Flags { get; set; }
        public string Value { get; set; }

        internal static CustomLog Deserialize(SimpitStream input)
        {
            return new CustomLog()
            {
                Flags = (FlagsEnum)input.ReadByte(),
                Value = input.ReadString(input.Length - 1)
            };
        }
    }
}
