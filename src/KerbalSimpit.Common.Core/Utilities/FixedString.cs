using System;
using System.Runtime.InteropServices;

namespace KerbalSimpit.Common.Core.Utilities
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct FixedString
    {
        private const byte NullChar = (byte)'\0';
        private const int BufferSize = 32;

        private fixed byte _buffer[BufferSize];

        public string Value
        {
            get
            {
                int length = BufferSize;
                string value = string.Empty;
                for (uint i = 0; i < length; i++)
                {
                    if (_buffer[i] == NullChar)
                    {
                        break;
                    }

                    value += Convert.ToChar(_buffer[i]);
                }

                return value;
            }
            set
            {
                this.Clear();

                int length = Math.Min(value.Length, BufferSize);
                for (int i = 0; i < length; i++)
                {
                    _buffer[i] = Convert.ToByte(value[i]);
                }
            }
        }

        public FixedString(string value)
        {
            this.Value = value;
        }

        public void Clear()
        {
            for (uint i = 0; i < BufferSize; i++)
            {
                _buffer[i] = NullChar;
            }
        }

        public override string ToString()
        {
            return this.Value;
        }

        public static implicit operator string(FixedString value)
        {
            return value.Value;
        }
    }
}
