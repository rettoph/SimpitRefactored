using KerbalSimpit.Core.Enums;
using KerbalSimpit.Core.Extensions;
using KerbalSimpit.Core.Messages;
using KerbalSimpit.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KerbalSimpit.Core.Services
{
    public class SimpitMessageService : IDisposable
    {
        private readonly Simpit _simpit;
        private readonly DoubleDictionary<byte, Type, SimpitMessageType> _incoming;
        private readonly DoubleDictionary<byte, Type, SimpitMessageType> _outgoing;

        public SimpitMessageService(Simpit simpit)
        {
            _simpit = simpit;
            _incoming = new DoubleDictionary<byte, Type, SimpitMessageType>();
            _outgoing = new DoubleDictionary<byte, Type, SimpitMessageType>();
        }

        public void Dispose()
        {
            _incoming.Clear();
            _outgoing.Clear();
        }

        public SimpitMessageType<T> RegisterIncomingType<T>(byte id, DeserializeSimpitMessageContentDelegate<T> deserializer)
            where T : ISimpitMessageContent
        {
            SimpitMessageType<T> configuration = new SimpitMessageType<T>(id, SimputMessageTypeEnum.Incoming, deserializer, null);
            _incoming.Add(configuration.Id, configuration.ContentType, configuration);
            return configuration;
        }

        public SimpitMessageType<T> RegisterIncomingType<T>(byte id)
            where T : unmanaged, ISimpitMessageContent
        {
            return this.RegisterIncomingType(id, input => input.ReadUnmanaged<T>());
        }

        public SimpitMessageType<T> RegisterOutogingType<T>(byte id, SerializeSimpitMessageContentDelegate<T> serializer)
            where T : ISimpitMessageContent
        {
            SimpitMessageType<T> configuration = new SimpitMessageType<T>(id, SimputMessageTypeEnum.Outgoing, null, serializer);
            _outgoing.Add(configuration.Id, configuration.ContentType, configuration);
            return configuration;
        }

        public SimpitMessageType<T> RegisterOutogingType<T>(byte id)
            where T : unmanaged, ISimpitMessageContent
        {
            return this.RegisterOutogingType<T>(id, (input, output) => output.WriteUnmanaged(input));
        }

        public bool TryGetIncomingType(byte id, out SimpitMessageType type)
        {
            return _incoming.TryGet(id, out type);
        }

        public bool TryGetIncomingType<T>(out SimpitMessageType<T> type)
            where T : ISimpitMessageContent
        {
            if (_incoming.TryGet(typeof(T), out SimpitMessageType uncasted) && uncasted is SimpitMessageType<T> casted)
            {
                type = casted;
                return true;
            }

            type = null;
            return false;
        }

        public bool TryGetOutgoingType(byte id, out SimpitMessageType type)
        {
            return _outgoing.TryGet(id, out type);
        }

        public bool TryGetOutgoingType<T>(out SimpitMessageType<T> type)
            where T : ISimpitMessageContent
        {
            if (_outgoing.TryGet(typeof(T), out SimpitMessageType uncasted) && uncasted is SimpitMessageType<T> casted)
            {
                type = casted;
                return true;
            }

            type = null;
            return false;
        }


        /// <summary>
        /// For thread-safety the caller is expected to provide
        /// the required buffer stream for intermediate decoding
        /// </summary>
        /// <param name="input"></param>
        /// <param name="buffer"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool TryDeserializeIncoming(SimpitStream input, SimpitStream buffer, out ISimpitMessage message)
        {
            buffer.Clear();

            try
            {
                bool success = COBSHelper.TryDecodeCOBS(input, buffer);
                if (success == false)
                {
                    _simpit.Logger.LogDebug("{0}::{1} - COBS decode failed.", nameof(SimpitMessageService), nameof(TryDeserializeIncoming));
                    message = null;
                    return false;
                }

                if (CheckSumHelper.ValidateCheckSum(buffer) == false)
                {
                    _simpit.Logger.LogDebug("{0}::{1} - Checksum validation failed.", nameof(SimpitMessageService), nameof(TryDeserializeIncoming));
                    message = null;
                    return false;
                }

                byte id = buffer.ReadByte();
                if (this.TryGetIncomingType(id, out SimpitMessageType type) == false)
                {
                    _simpit.Logger.LogError("{0}::{1} - Unknown message type id, {2}.", nameof(SimpitMessageService), nameof(TryDeserializeIncoming), id);
                    message = null;
                    return false;
                }

                _simpit.Logger.LogVerbose("{0}::{1} - Preparing to deserialize message type {2}.", nameof(SimpitMessageService), nameof(TryDeserializeIncoming), type);
                message = type.Deserialize(buffer);
                return true;
            }
            catch(Exception ex)
            {
                _simpit.Logger.LogError(ex, "{0}::{1} - Exception deserializing incoming message.", nameof(SimpitMessageService), nameof(TryDeserializeIncoming));
                message = default;
                return false;
            }
            finally
            {
                input.Clear();;
            }
        }

        public bool TrySerializeOutgoing(ISimpitMessage input, SimpitStream output, SimpitStream buffer)
        {
            output.Clear();
            buffer.Clear();

            try
            {
                buffer.Write(input.Type.Id);
                input.Type.Serialize(input, buffer);
                buffer.WriteCheckSum();

                bool success = COBSHelper.TryEncodeCOBS(buffer, output);
                return success;
            }
            catch(Exception ex)
            {
                _simpit.Logger.LogError(ex, "{0}::{1} - Exception serializing outgoing message.", nameof(SimpitMessageService), nameof(TrySerializeOutgoing));
                return false;
            }
        }

        public ISimpitMessage<T> CreateOutgoing<T>(T content)
            where T : ISimpitMessageContent
        {
            if(this.TryGetOutgoingType<T>(out SimpitMessageType<T> type) == true)
            {
                return new SimpitMessage<T>(type, content);
            }

            throw new InvalidOperationException($"{nameof(SimpitMessageService)}::{nameof(CreateOutgoing)} - Unknown outgoing message type {typeof(T).Name}.");
        }
    }
}
