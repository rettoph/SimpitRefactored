using KerbalSimpit.Common.Core;
using KerbalSimpit.Common.Core.Utilities;
using KerbalSimpit.Core.Enums;
using KerbalSimpit.Core.Peers;
using System;

namespace KerbalSimpit.Core
{
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

        internal abstract OutgoingData CreateOutgoingData();
    }

    public sealed class SimpitMessageType<TContent> : SimpitMessageType
        where TContent : unmanaged, ISimpitMessageData
    {

        internal SimpitMessageType(
            byte id,
            SimputMessageTypeEnum type) : base(id, type, typeof(TContent))
        {
        }

        internal override void Serialize(ISimpitMessage input, SimpitStream output)
        {
            if (input is SimpitMessage<TContent> casted)
            {
                output.WriteUnmanaged(casted.Data);
            }
        }

        internal override ISimpitMessage Deserialize(SimpitStream input)
        {
            // TODO: Maybe someday pool and reuse these message instances?
            TContent content = input.ReadUnmanaged<TContent>();
            return new SimpitMessage<TContent>(this, content);
        }

        internal override void TryEnqueueOutgoingData(SimpitPeer peer, int lastChangeId, Simpit simpit, bool force)
        {
            OutgoingData<TContent> data = simpit.GetOutgoingData(this);
            lock (data)
            {
                if (force == false && data.ChangeId == lastChangeId)
                {
                    return;
                }

                peer.EnqueueOutgoingSubscription(this, data);
            }
        }

        internal override void TryEnqueueOutgoingData(SimpitPeer peer, Simpit simpit)
        {
            OutgoingData<TContent> data = simpit.GetOutgoingData(this);
            lock (data)
            {
                peer.EnqueueOutgoing(data.Value);
            }
        }

        internal override OutgoingData CreateOutgoingData()
        {
            return OutgoingData<TContent>.Create();
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
