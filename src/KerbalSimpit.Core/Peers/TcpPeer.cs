using KerbalSimpit.Common.Core;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace KerbalSimpit.Core.Peers
{
    public class TcpPeer : SimpitPeer
    {
        private readonly TcpListener _listener;
        private NetworkStream _stream;

        public TcpPeer(int port) : base($"{port}")
        {
            _listener = new TcpListener(IPAddress.Any, port);
        }

        protected override bool TryOpen()
        {
            _listener.Start();

            for (int i = 0; i < 5; i++)
            {
                if (_listener.Pending() == false)
                {
                    Thread.Sleep(1000);
                    continue;
                }

                _stream = new NetworkStream(_listener.AcceptSocket());
                return true;
            }

            return false;
        }

        protected override bool TryClose()
        {
            _stream?.Dispose();
            _listener.Stop();

            return true;
        }

        protected override bool TryReadByte(out byte value)
        {
            if (_stream.DataAvailable == false)
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
