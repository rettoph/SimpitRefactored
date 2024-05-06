using System;

namespace KerbalSimpit.Core.Messages
{
    public struct CustomLog : ISimpitMessageData
    {
        [Flags]
        public enum FlagsEnum
        {
            None = 0,
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
