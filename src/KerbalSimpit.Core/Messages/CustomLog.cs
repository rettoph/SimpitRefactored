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
        public string Value { get; set; }

        internal static CustomLog Deserialize(SimpitStream input)
        {
            input.Skip(1);

            return new CustomLog()
            {
                Value = input.ReadString(input.Length - 1)
            };
        }
    }
}
