using KerbalSimpit.Core.Enums;
using KerbalSimpit.Core.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KerbalSimpit.Core
{
    public delegate T DeserializeSimpitMessageContentDelegate<T>(SimpitStream input)
        where T : ISimpitMessageContent;

    public delegate void SerializeSimpitMessageContentDelegate<T>(in T input, SimpitStream output)
        where T : ISimpitMessageContent;

    public abstract class SimpitMessageType
    {
        public readonly byte Id;
        public readonly SimputMessageTypeEnum Type;
        public readonly Type ContentType;

        internal SimpitMessageType(byte id, SimputMessageTypeEnum type, Type contentType)
        {
            ThrowIf.Type.IsNotAssignableTo<ISimpitMessageContent>(contentType);

            this.Id = id;
            this.Type = type;
            this.ContentType = contentType;
        }

        /// <summary>
        /// For thread-safety its expected that the deserialization caller
        /// provides the buffer stream for intermediate decoding
        /// </summary>
        /// <param name="input"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        internal abstract ISimpitMessage Deserialize(SimpitStream input);
    }

    public sealed class SimpitMessageType<TContent> : SimpitMessageType
        where TContent : ISimpitMessageContent
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

        internal override ISimpitMessage Deserialize(SimpitStream input)
        {
            // TODO: Maybe someday pool and reuse these message intnaces?
            TContent content = _deserializer(input);
            return new SimpitMessage<TContent>(this, content);
        }

        private static void NotImplementedSerializer(in TContent input, SimpitStream output)
        {
            throw new NotImplementedException();
        }

        private static TContent NotImplementedDeserializer(SimpitStream input)
        {
            throw new NotImplementedException();
        }
    }
}
