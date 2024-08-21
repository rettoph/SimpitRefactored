namespace SimpitRefactored.Common.Core
{
    public static class Constants
    {
        public const int MaximumMessageSize = 32;

        /// <summary>
        /// Indicates the end of a stream has been read
        /// </summary>
        public const int EndOfData = -1;

        /// <summary>
        /// Indicates the end of a continuous message
        /// </summary>
        public const int EndOfMessage = 0;

        public static class MessageTypeIds
        {
            public static class Incoming
            {
                public const byte Synchronisation = 0;
                public const byte EchoRequest = 1;
                public const byte EchoResponse = 2;
                public const byte CloseSerialPort = 7;
                public const byte RegisterHandler = 8;
                public const byte DeregisterHandler = 9;
                public const byte CustomLog = 25;
                public const byte RequestMessage = 29;
            }

            public static class Outgoing
            {
                public const byte HandshakeMessage = 0;
                public const byte EchoResponse = 2;
            }
        }
    }
}
