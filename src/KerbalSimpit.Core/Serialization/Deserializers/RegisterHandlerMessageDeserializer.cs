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
    internal class RegisterHandlerMessageDeserializer : BaseSimpitMessageDeserializer<RegisterHandlerMessage>
    {
        public override SimpitMessageId MessageId => MessageIds.Inbound.RegisterHandler;

        protected override bool TryDeserialize(SimpitStream stream, out RegisterHandlerMessage message)
        {
            message = new RegisterHandlerMessage()
            {
                MessageIds = stream.ReadAll(out int offset, out int count).Skip(offset).Take(count).ToArray()
            };

            return true;
        }
    }
}
