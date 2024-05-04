using System;

namespace KerbalSimpit.Core.Serialization.Serializers
{
    public abstract class BaseSimpitMessageSerializer<T> : ISimpitMessageSerializer
        where T : ISimpitMessage
    {
        public Type MessageType => typeof(T);

        public abstract byte MessageId { get; }

        void ISimpitMessageSerializer.Serialize(SimpitStream stream, ISimpitMessage message)
        {
            // TODO: remove boxing
            if (message is T casted)
            {
                this.Serialize(stream, casted);
            }
        }

        protected abstract void Serialize(SimpitStream stream, T message);
    }
}
