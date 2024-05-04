using KerbalSimpit.Core.Constants;
using KerbalSimpit.Core.Extensions;
using KerbalSimpit.Core.Peers;
using KerbalSimpit.Core.Serialization;
using KerbalSimpit.Core.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace KerbalSimpit.Core
{
    public partial class SimpitMessageSerializer
    {
        private static DoubleDictionary<byte, Type, ISimpitMessageSerializer> _serializers;
        private static DoubleDictionary<byte, Type, ISimpitMessageDeserializer> _deserializers;

        private readonly ISimpitLogger _logger;
        private readonly SimpitStream _decodeBuffer;
        private readonly SimpitStream _serializeBuffer;


        public SimpitMessageSerializer(ISimpitLogger logger)
        {
            _logger = logger;
            _serializeBuffer = new SimpitStream();
            _decodeBuffer = new SimpitStream();
        }

        public bool TrySerialize(SimpitStream stream, ISimpitMessage message)
        {
            if (_serializers.TryGet(message.GetType(), out ISimpitMessageSerializer serializer) == false)
            {
                _logger.LogError("{0}::{1} - Unknown message type, {2}.", nameof(SimpitMessageSerializer), nameof(TrySerialize), message.GetType().Name);
                message = null;
                return false;
            }

            _serializeBuffer.Write(serializer.MessageId);
            serializer.Serialize(_serializeBuffer, message);
            _serializeBuffer.WriteCheckSum();

            return COBSHelper.TryEncodeCOBS(_serializeBuffer, stream);
        }

        public bool TryDeserialize(SimpitStream stream, out ISimpitMessage message)
        {
            try
            {
                bool success = COBSHelper.TryDecodeCOBS(stream, _decodeBuffer);
                if (success == false)
                {
                    _logger.LogDebug("{0}::{1} - COBS decode failed.", nameof(SimpitMessageSerializer), nameof(TryDeserialize));
                    message = null;
                    return false;
                }

                if(this.ValidateCheckSum(_decodeBuffer) == false)
                {
                    _logger.LogDebug("{0}::{1} - Checksum validation failed.", nameof(SimpitMessageSerializer), nameof(TryDeserialize));
                    message = null;
                    return false;
                }

                byte id = _decodeBuffer.ReadByte();
                if(_deserializers.TryGet(id, out ISimpitMessageDeserializer deserializer) == false)
                {
                    _logger.LogError("{0}::{1} - Unknown message id, {2}.", nameof(SimpitMessageSerializer), nameof(TryDeserialize), id);
                    message = null;
                    return false;
                }

                return deserializer.TryDeserialize(_decodeBuffer, out message);
            }
            finally
            {
                stream.Clear();
            }
        }

        private bool ValidateCheckSum(SimpitStream data)
        {
            byte expected = data.Pop();
            byte calculated = CheckSumHelper.CalculateCheckSum(data);

            if (calculated != expected)
            {
                _logger.LogVerbose("{0}::{1} - Checksum validation failed, expcected {2} but got {3}.", nameof(SimpitMessageSerializer), nameof(ValidateCheckSum), expected, calculated);
                return false;
            }

            return true;
        }

        static SimpitMessageSerializer()
        {
            _serializers = new DoubleDictionary<byte, Type, ISimpitMessageSerializer>();
            _deserializers = new DoubleDictionary<byte, Type, ISimpitMessageDeserializer>();

            SimpitMessageSerializer.RegisterCoreMessages();
        }

        public static void RegisterSerializer(ISimpitMessageSerializer serializer)
        {
            _serializers.Add(serializer.MessageId, serializer.MessageType, serializer);
        }

        public static void RegisterDeserializer(ISimpitMessageDeserializer deserializer)
        {
            _deserializers.Add(deserializer.MessageId, deserializer.MessageType, deserializer);
        }

        public static void RegisterSerializer<T>(byte messageId)
            where T : unmanaged, ISimpitMessage
        {
            RegisterSerializer(new UnmanagedMessageSerializer<T>(messageId));
        }

        public static void RegisterDeserializer<T>(byte messageId)
            where T : unmanaged, ISimpitMessage
        {
            RegisterDeserializer(new UnmanagedMessageDeserializer<T>(messageId));
        }
    }
}
