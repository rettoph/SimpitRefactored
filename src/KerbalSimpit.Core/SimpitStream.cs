using KerbalSimpit.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KerbalSimpit.Core
{
    public sealed class SimpitStream
    {
        private const int BufferSize = 256;

        private byte[] _buffer;

        private int _writeIndex;
        private int _readIndex;

        public int Length => _writeIndex - _readIndex;

        public SimpitStreamModeEnum Mode { get; private set; }

        public SimpitStream()
        {
            _buffer = new byte[BufferSize];
        }

        public void Clear()
        {
            this.Mode = SimpitStreamModeEnum.None;

            _writeIndex = 0;
            _readIndex = 0;
        }

        public void Write(byte value)
        {
            if(this.SetMode(SimpitStreamModeEnum.Write, ref _writeIndex))
            {
                _readIndex = 0;
            }
            
            _buffer[_writeIndex] = value;
            _writeIndex = (_writeIndex + 1) % _buffer.Length;
        }

        public void Write(byte[] values)
        {
            for (int i = 0; i < values.Length; i++)
            {
                this.Write(values[i]);
            }
        }

        public void Write(byte value, int index)
        {
            if (this.SetMode(SimpitStreamModeEnum.Write, ref _writeIndex))
            {
                _readIndex = 0;
            }

            _buffer[index] = value;
            _writeIndex = Math.Max(_writeIndex, index);
        }

        public unsafe void WriteUnmanaged<T>(T value)
            where T : unmanaged
        {
            T* tPtr = &value;
            byte* bytePtr = (byte*)tPtr;

            for (int i = 0; i < sizeof(T); i++)
            {
                this.Write(bytePtr[i]);
            }
        }

        public byte Pop()
        {
            if(_writeIndex <= 0)
            {
                throw new InvalidOperationException();
            }

            return _buffer[--_writeIndex];
        }

        public bool TryReadByte(out byte value)
        {
            this.SetMode(SimpitStreamModeEnum.Read, ref _readIndex);

            if (this.Length < sizeof(byte))
            {
                value = default;
                return false;
            }

            value = _buffer[_readIndex++];
            return true;
        }

        public unsafe bool TryReadUnmanaged<T>(out T value)
            where T : unmanaged
        {
            if (this.Length < sizeof(T))
            {
                value = default;
                return false;
            }

            fixed (byte* bytePtr = &_buffer[_readIndex])
            {
                T* tPtr = (T*)bytePtr;
                value = tPtr[0];

                _readIndex += sizeof(T);

                return true;
            }
        }

        public unsafe T ReadUnmanaged<T>()
            where T : unmanaged
        {
            if (this.Length < sizeof(T))
            {
                throw new InvalidOperationException();
            }

            fixed (byte* bytePtr = &_buffer[_readIndex])
            {
                T* tPtr = (T*)bytePtr;
                T value = tPtr[0];

                _readIndex += sizeof(T);

                return value;
            }
        }

        public byte[] ReadAll(out int offset, out int count)
        {
            this.SetMode(SimpitStreamModeEnum.Read, ref _readIndex);

            offset = _readIndex;
            count = this.Length;

            _readIndex += this.Length;

            return _buffer;
        }

        public byte ReadByte()
        {
            this.SetMode(SimpitStreamModeEnum.Read, ref _readIndex);

            if (this.Length < sizeof(byte))
            {
                throw new InvalidOperationException();
            }

            return _buffer[_readIndex++];
        }

        public int ReadInt32()
        {
            int value = this.ConvertBytes(sizeof(int), BitConverter.ToInt32);
            return value;
        }

        public float ReadSingle()
        {
            float value = this.ConvertBytes(sizeof(float), BitConverter.ToSingle);
            return value;
        }

        public bool ReadBoolean()
        {
            bool value = this.ConvertBytes(sizeof(int), BitConverter.ToBoolean);
            return value;
        }

        public void Skip(int count)
        {
            switch(this.Mode)
            {
                case SimpitStreamModeEnum.None:
                    throw new InvalidOperationException();
                case SimpitStreamModeEnum.Write:
                    _writeIndex += count;
                    break;
                case SimpitStreamModeEnum.Read:
                    _readIndex += count;
                    break;
            }
        }

        /// <summary>
        /// Return a string of the given length. This will do
        /// straight byte to char to string conversion
        /// </summary>
        /// <param name="length">If zero it will read the remainder of the stream</param>
        /// <returns></returns>
        public string ReadString(int length = -1)
        {
            if(length == -1)
            {
                length = this.Length - 1;
            }

            string value = Encoding.UTF8.GetString(_buffer, _readIndex, length);
            _readIndex += length;

            return value;
        }

        public byte FastReadByte()
        {
            this.SetMode(SimpitStreamModeEnum.Read, ref _readIndex);

            return _buffer[_readIndex++];
        }

        public int FastReadInt32()
        {
            int value = this.FastConvertBytes(sizeof(int), BitConverter.ToInt32);
            return value;
        }

        public float FastReadSingle()
        {
            float value = this.FastConvertBytes(sizeof(float), BitConverter.ToSingle);
            return value;
        }

        public bool FastReadBoolean()
        {
            bool value = this.FastConvertBytes(sizeof(int), BitConverter.ToBoolean);
            return value;
        }

        public byte Peek(int index)
        {
            return _buffer[index];
        }

        private T ConvertBytes<T>(int size, Func<byte[], int, T> converter)
        {
            this.SetMode(SimpitStreamModeEnum.Read, ref _readIndex);

            if (this.Length < size)
            {
                throw new InvalidOperationException();
            }

            T value = converter(_buffer, _readIndex);
            _readIndex += size;

            return value;
        }

        private T FastConvertBytes<T>(int size, Func<byte[], int, T> converter)
        {
            this.SetMode(SimpitStreamModeEnum.Read, ref _readIndex);

            T value = converter(_buffer, _readIndex);
            _readIndex += size;

            return value;
        }

        private bool SetMode(SimpitStreamModeEnum mode, ref int index)
        {
            if(this.Mode == mode)
            {
                return false;
            }

            index = 0;
            this.Mode = mode;

            return true;
        }
    }
}
