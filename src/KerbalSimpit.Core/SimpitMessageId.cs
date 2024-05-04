using KerbalSimpit.Core.Enums;
using KerbalSimpit.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KerbalSimpit.Core
{
    public abstract class SimpitMessageId
    {
        public readonly byte Value;
        public readonly Type Type;
        public readonly SimputMessageIdDirectionEnum Direction;

        internal SimpitMessageId(byte value, Type type, SimputMessageIdDirectionEnum direction)
        {
            this.Value = value;
            this.Type = type;
            this.Direction = direction;
        }

        private static DoubleDictionary<byte, Type, SimpitMessageId> _inbound = new DoubleDictionary<byte, Type, SimpitMessageId>();
        private static DoubleDictionary<byte, Type, SimpitMessageId> _outbound = new DoubleDictionary<byte, Type, SimpitMessageId>();

        public static bool TryGetByValue(byte value, SimputMessageIdDirectionEnum direction, out SimpitMessageId id)
        {
            switch (direction)
            {
                case SimputMessageIdDirectionEnum.Inbound:
                    return _inbound.TryGet(value, out id);

                case SimputMessageIdDirectionEnum.Outbound:
                    return _outbound.TryGet(value, out id);

                default:
                    throw new InvalidOperationException();
            }
        }

        public static SimpitMessageId<T> GetOrCreate<T>(byte value, SimputMessageIdDirectionEnum direction)
            where T : ISimpitMessage
        {
            switch (direction)
            {
                case SimputMessageIdDirectionEnum.Inbound:
                    return GetOrCreate<T>(_inbound, value, direction);

                case SimputMessageIdDirectionEnum.Outbound:
                    return GetOrCreate<T>(_outbound, value, direction);

                default:
                    throw new InvalidOperationException();
            }
        }

        private static SimpitMessageId<T> GetOrCreate<T>(DoubleDictionary<byte, Type, SimpitMessageId> cache, byte value, SimputMessageIdDirectionEnum direction)
            where T : ISimpitMessage
        {
            if (cache.TryGet(value, out SimpitMessageId uncasted))
            {
                if (uncasted is SimpitMessageId<T> casted)
                {
                    return casted;
                }
                else
                {
                    throw new NotImplementedException();
                }
            }

            SimpitMessageId<T> id = new SimpitMessageId<T>(value, typeof(T), direction);
            cache.Add(value, typeof(T), id);
            return id;
        }
    }

    public sealed class SimpitMessageId<T> : SimpitMessageId
    {
        internal SimpitMessageId(byte value, Type type, SimputMessageIdDirectionEnum direction) : base(value, type, direction)
        {
        }
    }
}
