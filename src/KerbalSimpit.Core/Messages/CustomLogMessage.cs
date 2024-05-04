using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KerbalSimpit.Core.Messages
{
    public struct CustomLogMessage : ISimpitMessage
    {
        public string Value { get; set; }
    }
}
