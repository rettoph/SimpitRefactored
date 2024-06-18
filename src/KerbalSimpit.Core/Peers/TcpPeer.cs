using KerbalSimpit.Common.Core;
using System.Net.Sockets;

namespace KerbalSimpit.Core.Peers
{
    public class TcpPeer : SimpitPeer
    {
        private readonly string _host;
        private readonly int _port;
        private readonly TcpClient _client;
        private NetworkStream _stream;

        public TcpPeer(string host, int port) : base($"{host}:{port}")
        {
            _host = host;
            _port = port;
            _client = new TcpClient();
        }

        protected override bool TryOpen()
        {
            _client.Connect(_host, _port);
            if (_client.Connected == false)
            {
                return false;
            }

            _stream = _client.GetStream();
            return true;
        }

        protected override bool TryClose()
        {
            _client.Close();

            return true;
        }

        protected override bool TryReadByte(out byte value)
        {
            if (_client.Available == 0)
            {
                value = default;
                return false;
            }

            int readResult = _stream.ReadByte();
            if (readResult == Constants.EndOfData)
            {
                value = default;
                return false;
            }

            value = (byte)readResult;
            return true;
        }

        protected override void Write(byte[] data, int offset, int length)
        {
            _stream.Write(data, offset, length);
        }
    }
}
