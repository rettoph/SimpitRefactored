using System.Collections.Concurrent;
using System.IO.Ports;
using System.Net.Sockets;
using KerbalSimpit.Common.Core;

namespace KerbalSimpit.Proxy.SerialThroughTCP
{
    public sealed class SimpitProxy
    {
        private object _lock = new object();

        private SimpitProxyConfiguration _configuration;
        private bool _running;

        private readonly SerialPort _serial;
        private TcpClient? _client;
        private NetworkStream? _stream;

        private Task _loop;

        private SimpitStream _buffer;

        public SimpitProxy(SimpitProxyConfiguration configuration)
        {
            _configuration = configuration;

            _serial = new SerialPort(_configuration.SerialPortName, _configuration.SerialBaudRate);
            _serial.DtrEnable = true;

            _loop = new Task(async () => await this.Loop());
            _buffer = new SimpitStream();
        }

        public Task StartAsync()
        {
            Console.WriteLine($"Starting Proxy {_configuration.SerialPortName} <--> {_configuration.TcpClientHost}:{_configuration.TcpClientPort}");

            _running = true;

            _loop.Start();

            return Task.CompletedTask;
        }

        private void Reset()
        {
            _client?.Dispose();
            _stream?.Dispose();

            _client = null;
            _stream = null;

            _serial.Close();
        }

        private bool VerifyTcpConnection()
        {
            bool TryEstablishTcpConnection()
            {
                lock(_lock)
                {
                    if(_stream is not null)
                    {
                        return true;
                    }

                    try 
                    {
                        this.Reset();

                        LogInformation($"{nameof(SimpitProxy)}::{nameof(TryEstablishTcpConnection)} - Attempting to establish TCP connection with {_configuration.TcpClientHost}...");
                        _client = new TcpClient();
                        _client.Connect(_configuration.TcpClientHost, _configuration.TcpClientPort);
                        _stream = _client.GetStream();

                        LogInformation($"{nameof(SimpitProxy)}::{nameof(TryEstablishTcpConnection)} - TCP Connection established.");

                        return true;
                    }
                    catch(Exception ex)
                    {
                        LogException($"{nameof(SimpitProxy)}::{nameof(TryEstablishTcpConnection)}", ex);

                        return false;
                    }
                }
            }

            try 
            {
                if(_client is null)
                {
                    return TryEstablishTcpConnection();
                }

                if(_client.Connected == false)
                {
                    return TryEstablishTcpConnection();
                }

                if(_stream is null)
                {
                    return TryEstablishTcpConnection();
                }

                return true;
            }
            catch(Exception ex)
            {
                LogException($"{nameof(SimpitProxy)}::{nameof(TryEstablishTcpConnection)}", ex);

                return TryEstablishTcpConnection();
            }
        }

        private bool VerifySerialConnection()
        {
            bool TryEstablishSerialConnection()
            {
                lock(_lock)
                {
                    try 
                    {

                        if(_serial.IsOpen == true)
                        {
                            return true;
                        }

                        LogInformation($"{nameof(SimpitProxy)}::{nameof(Loop)} - Begin Serial open...");
                        _serial.Open();
                        LogInformation($"{nameof(SimpitProxy)}::{nameof(Loop)} - End Serial open.");

                        return true;

                    }
                    catch(Exception ex)
                    {
                        _serial.Close();

                        LogException($"{nameof(SimpitProxy)}::{nameof(TryEstablishSerialConnection)}", ex);

                        return false;               
                    }
                }
            }

            if(_serial.IsOpen == false)
            {
                return TryEstablishSerialConnection();
            }

            return true;
        }


        private async Task Loop()
        {
            while(_running)
            {
                try
                {
                    if(this.VerifyTcpConnection() == false)
                    {
                        await Task.Delay(2500);

                        continue;
                    }

                    if (this.VerifySerialConnection() == false)
                    {
                        await Task.Delay(2500);

                        continue;
                    }

                    // Read Serial -> Write TCP
                    while (_serial.BytesToRead > 0)
                    {
                        int value = _serial.ReadByte();
                        if (value == -1)
                        {
                            continue;
                        }

                        _buffer.Write((byte)value);

                        if(value == 0)
                        {
                            byte[] data = _buffer.ReadAll(out int offset, out int count);
                            _stream!.Write(data, offset, count);

                            _buffer.Clear();
                        }
                    }

                    // Read TCP -> Write Serial
                    while (_stream!.DataAvailable == true)
                    {
                        int value = _stream.ReadByte();
                        if (value == -1)
                        {
                            continue;
                        }

                        _buffer.Write((byte)value);

                        if(value == 0)
                        {
                            byte[] data = _buffer.ReadAll(out int offset, out int count);
                            _serial!.Write(data, offset, count);

                            _buffer.Clear();
                        }
                    }
                }
                catch(Exception ex)
                {
                    this.Reset();

                    LogException($"{nameof(SimpitProxy)}::{nameof(Loop)}", ex);
                    await Task.Delay(2500);
                }

                await Task.Delay(16);
            }
        }

        private void LogException(string message, Exception ex)
        {
            ConsoleColor foreground = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{_configuration.SerialPortName}:{message} => {ex.GetType().Name}: {ex.Message}");
            Console.ForegroundColor = foreground;
        }

        private void LogInformation(string message)
        {
            ConsoleColor foreground = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"{_configuration.SerialPortName}:{message}");
            Console.ForegroundColor = foreground;
        }
    }
}
