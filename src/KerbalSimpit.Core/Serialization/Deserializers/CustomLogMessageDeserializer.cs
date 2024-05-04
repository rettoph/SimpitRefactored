using KerbalSimpit.Core.Enums;
using KerbalSimpit.Core.Messages;
using KerbalSimpit.Core.Serialization.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KerbalSimpit.Core.Serialization.Deserializers
{
    internal sealed class CustomLogMessageDeserializer : BaseSimpitMessageDeserializer<CustomLogMessage>
    {
        public override byte MessageId => (byte)CoreInboundMessageIdEnum.CustomLog;

        protected override bool TryDeserialize(SimpitStream stream, out CustomLogMessage message)
        {
            stream.Skip(1);

            message = new CustomLogMessage()
            {
                Value = stream.ReadString(stream.Length - 1)
            };

            return true;
        }
    }
}
