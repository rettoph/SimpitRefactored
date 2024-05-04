using KerbalSimpit.Core.Messages;
using KerbalSimpit.Core.Serialization.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KerbalSimpit.Core.Serialization
{
    internal sealed class UnmanagedMessageSerializer<T> : BaseSimpitMessageSerializer<T>
        where T : unmanaged, ISimpitMessage
    {
        private readonly byte _messageId;

        public override byte MessageId => _messageId;

        public UnmanagedMessageSerializer(byte headerId)
        {
            _messageId = headerId; ;
        }

        protected override void Serialize(SimpitStream stream, T message)
        {
            stream.WriteUnmanaged(message);
        }
    }
}
