using System.IO.Ports;
using System.Net.Sockets;

namespace KerbalSimpit.Proxy.SerialThroughTCP
{
    public sealed class SimpitProxy
    {
        private SimpitProxyConfiguration _configuration;

        private readonly SerialPort _serial;
        private readonly TcpClient _client;
        private NetworkStream? _stream;

        private byte[] _buffer = new byte[64];
        private int _index = 0;

        public SimpitProxy(SimpitProxyConfiguration configuration)
        {
            _configuration = configuration;

            _serial = new SerialPort(_configuration.SerialPortName, _configuration.SerialBaudRate);
            _client = new TcpClient();
        }

        public async Task RunAsync()
        {
            Console.WriteLine($"Starting Proxy {_configuration.SerialPortName} <--> {_configuration.TcpClientHost}:{_configuration.TcpClientPort}");
            while (true)
            {
                try
                {
                    await this.Loop();
                    await Task.Delay(50);
                }
                catch (Exception ex)
                {
                    ConsoleColor foreground = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Exception: {ex.Message}");
                    Console.ForegroundColor = foreground;

                    await Task.Delay(1000);
                }
            }
        }

        private async Task Loop()
        {
            if (_client.Connected == false)
            {
                _stream?.Dispose();
                _stream = null;

                await _client.ConnectAsync(_configuration.TcpClientHost, _configuration.TcpClientPort);
                _stream = _client.GetStream();
            }

            if (_serial.IsOpen == false)
            {
                _serial.Open();
            }

            if (_stream is null)
            {
                Console.WriteLine("Stream is null");
                return;
            }

            // Read Serial -> Write TCP
            while (_serial.BytesToRead > 0)
            {
                int value = _serial.ReadByte();
                if (value == -1)
                {
                    continue;
                }

                _stream.WriteByte((byte)value);
            }


            // Read TCP -> Write Serial
            while (_stream.DataAvailable == true)
            {
                int value = _stream.ReadByte();
                if (value == -1)
                {
                    continue;
                }

                _buffer[_index++] = (byte)value;

                if (value == 0)
                {
                    _serial.Write(_buffer, 0, _index);
                    _index = 0;
                }
            }
        }
    }
}
