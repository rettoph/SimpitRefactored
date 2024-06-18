using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace KerbalSimpit.Common.Core.Utilities
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct FixedBuffer
    {
        private const int BufferSize = Constants.MaximumMessageSize;
        private fixed byte _buffer[BufferSize];

        public T Get<T>(int index)
            where T : unmanaged
        {
            int size = sizeof(T);
            int length = BufferSize / size;

            if (index >= length)
            {
                throw new ArgumentOutOfRangeException();
            }

            fixed (byte* bufferPtr = _buffer)
            {
                T* ptr = (T*)bufferPtr;

                return ptr[index];
            }
        }

        public void Set<T>(int index, T value)
            where T : unmanaged
        {
            int size = sizeof(T);
            int length = BufferSize / size;

            if (index >= length)
            {
                throw new ArgumentOutOfRangeException();
            }

            fixed (byte* bufferPtr = _buffer)
            {
                T* ptr = (T*)bufferPtr;

                ptr[index] = value;
            }
        }

        public void Clear()
        {
            this.Fill<byte>(default);
        }

        public void Copy(FixedBuffer source)
        {
            for (int i = 0; i < BufferSize; i++)
            {
                _buffer[i] = source._buffer[i];
            }
        }

        public void Fill<T>(T value)
            where T : unmanaged
        {
            int size = sizeof(T);
            int length = BufferSize / size;

            fixed (byte* bufferPtr = _buffer)
            {
                T* ptr = (T*)bufferPtr;

                for (uint i = 0; i < length; i++)
                {
                    ptr[i] = value;
                }
            }
        }

        public IList<T> ToList<T>(T deliminator = default)
            where T : unmanaged
        {
            List<T> items = new List<T>();

            int size = sizeof(T);
            int length = BufferSize / size;

            fixed (byte* bufferPtr = _buffer)
            {
                T* ptr = (T*)bufferPtr;

                for (uint i = 0; i < length; i++)
                {
                    if (Unmanaged.Equals(ptr[i], deliminator) == true)
                    {
                        break;
                    }

                    items.Add(ptr[i]);
                }
            }

            return items;
        }
    }
}
