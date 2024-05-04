using KerbalSimpit.Core.Messages;
using KerbalSimpit.Core.Serialization.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KerbalSimpit.Core.Serialization
{
    internal sealed class UnmanagedMessageDeserializer<T> : BaseSimpitMessageDeserializer<T>
        where T : unmanaged, ISimpitMessage
    {
        private readonly byte _messageId;

        public override byte MessageId => _messageId;

        public UnmanagedMessageDeserializer(byte headerId)
        {
            _messageId = headerId;
        }

        protected unsafe override bool TryDeserialize(SimpitStream stream, out T message)
        {
            if(stream.Length != sizeof(T))
            {
                throw new InvalidOperationException("Too much data recieved.");
            }

            return stream.TryReadUnmanaged(out message);
        }
    }
}
