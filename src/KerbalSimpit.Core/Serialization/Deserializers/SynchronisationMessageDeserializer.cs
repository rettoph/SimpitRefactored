using KerbalSimpit.Core.Constants;
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
    internal sealed class SynchronisationMessageDeserializer : BaseSimpitMessageDeserializer<SynchronisationMessage>
    {
        public override SimpitMessageId MessageId => MessageIds.Inbound.Synchronisation;

        protected override bool TryDeserialize(SimpitStream stream, out SynchronisationMessage message)
        {
            message = new SynchronisationMessage()
            {
                Type = (SynchronisationMessage.SynchronisationMessageTypeEnum)stream.ReadByte(),
                Version = stream.ReadString(stream.Length - 1)
            };

            return true;
        }
    }
}
