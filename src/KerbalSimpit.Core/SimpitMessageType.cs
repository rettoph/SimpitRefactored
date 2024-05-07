using KerbalSimpit.Core.Enums;
using KerbalSimpit.Core.Peers;
using KerbalSimpit.Core.Utilities;
using System;

namespace KerbalSimpit.Core
{
    public delegate T DeserializeSimpitMessageContentDelegate<T>(SimpitStream input)
        where T : ISimpitMessageData;

    public delegate void SerializeSimpitMessageContentDelegate<T>(T input, SimpitStream output)
        where T : ISimpitMessageData;

    public abstract class SimpitMessageType
    {
        public readonly byte Id;
        public readonly SimputMessageTypeEnum Type;
        public readonly Type DataType;

        internal SimpitMessageType(byte id, SimputMessageTypeEnum type, Type contentType)
        {
            ThrowIf.Type.IsNotAssignableTo<ISimpitMessageData>(contentType);

            this.Id = id;
            this.Type = type;
            this.DataType = contentType;
        }

        internal abstract void Serialize(ISimpitMessage input, SimpitStream output);
        internal abstract ISimpitMessage Deserialize(SimpitStream input);

        internal abstract void TryEnqueueOutgoingData(SimpitPeer peer, int lastChangeId, Simpit simpit, bool force);
        internal abstract void TryEnqueueOutgoingData(SimpitPeer peer, Simpit simpit);
    }

    public sealed class SimpitMessageType<TContent> : SimpitMessageType
        where TContent : ISimpitMessageData
    {
        private readonly DeserializeSimpitMessageContentDelegate<TContent> _deserializer;
        private readonly SerializeSimpitMessageContentDelegate<TContent> _serializer;

        internal SimpitMessageType(
            byte id,
            SimputMessageTypeEnum type,
            DeserializeSimpitMessageContentDelegate<TContent> deserializer,
            SerializeSimpitMessageContentDelegate<TContent> serializer) : base(id, type, typeof(TContent))
        {
            _deserializer = deserializer ?? NotImplementedDeserializer;
            _serializer = serializer ?? NotImplementedSerializer;
        }

        internal override void Serialize(ISimpitMessage input, SimpitStream output)
        {
            if (input is SimpitMessage<TContent> casted)
            {
                _serializer(casted.Data, output);
            }
        }

        internal override ISimpitMessage Deserialize(SimpitStream input)
        {
            // TODO: Maybe someday pool and reuse these message intnaces?
            TContent content = _deserializer(input);
            return new SimpitMessage<TContent>(this, content);
        }

        internal override void TryEnqueueOutgoingData(SimpitPeer peer, int lastChangeId, Simpit simpit, bool force)
        {
            Simpit.OutgoingData<TContent> data = simpit.GetOutgoingData(this);
            lock (data)
            {
                if (force == false && data.ChangeId == lastChangeId)
                {
                    return;
                }

                peer.EnqueueOutgoindSubscription(this, data);
            }
        }

        internal override void TryEnqueueOutgoingData(SimpitPeer peer, Simpit simpit)
        {
            Simpit.OutgoingData<TContent> data = simpit.GetOutgoingData(this);
            lock (data)
            {
                peer.EnqueueOutgoing(data.Value);
            }
        }

        public override string ToString()
        {
            return $"{this.Id}:{this.DataType.Name}";
        }

        private static void NotImplementedSerializer(TContent input, SimpitStream output)
        {
            throw new NotImplementedException();
        }

        private static TContent NotImplementedDeserializer(SimpitStream input)
        {
            throw new NotImplementedException();
        }
    }
}
