using System;

namespace KerbalSimpit.Core.Serialization.Serializers
{
    public abstract class BaseSimpitMessageDeserializer<T> : ISimpitMessageDeserializer
        where T : ISimpitMessage
    {
        public abstract SimpitMessageId MessageId { get; }

        bool ISimpitMessageDeserializer.TryDeserialize(SimpitStream stream, out ISimpitMessage message)
        {
            if(this.TryDeserialize(stream, out T casted) == false)
            {
                message = default;
                return false;
            }

            // TODO: remove boxing
            message = casted;
            return true;

        }

        protected abstract bool TryDeserialize(SimpitStream stream, out T message);
    }
}
