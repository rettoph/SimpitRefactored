using KerbalSimpit.Core.Constants;
using KerbalSimpit.Core.Enums;
using KerbalSimpit.Core.Peers;
using KerbalSimpit.Core.Utilities;
using System;
using System.IO.Ports;
using System.Threading;

namespace KerbalSimpit.Core.Peers
{
    public class SerialPeer : SimpitPeer
    {
        private readonly SerialPort _port;
        private readonly Thread _inboundThread;
        private readonly Thread _outboundThread;


        private bool _inboundRunning;
        private bool _outboundRunning;

        public override bool Running => _inboundRunning || _outboundRunning;

        public SerialPeer(string name, int baudRate) : base()
        {
            _port = new SerialPort(name, baudRate, Parity.None, 8, StopBits.One);
            _inboundThread = new Thread(this.InboundLoop);
            _outboundThread = new Thread(this.OutboundLoop);

            // To allow communication from a Pi Pico, the DTR seems to be mandatory to allow the connection
            // This does not seem to prevent communication from Arduino.
            _port.DtrEnable = true;
        }

        protected override void Start()
        {
            if(this.Running == true)
            {
                throw new InvalidOperationException($"{nameof(SerialPeer)}::{nameof(Start)} - Already running");
            }

            _port.Open();
            _inboundThread.Start();
            _outboundThread.Start();
        }

        protected override void Stop()
        {
            if (this.Running == false)
            {
                throw new InvalidOperationException($"{nameof(SerialPeer)}::{nameof(Start)} - Not running");
            }

            _inboundRunning = false;
            _outboundRunning = false;
            _port.Close();
        }

        private void InboundLoop()
        {
            this.logger.LogDebug("Inbound thread for port {0} starting.", _port.PortName);
            _inboundRunning = true;

            while (this.cancellationToken.IsCancellationRequested == false && _inboundRunning)
            {
                while(_port.BytesToRead > 0 && this.cancellationToken.IsCancellationRequested == false && _inboundRunning)
                {
                    int data = _port.ReadByte();

                    if (data == SpecialBytes.EndOfData)
                    { // This should not happen thanks to the while check above
                        throw new NotImplementedException();
                    }

                    this.IncomingDataRecieved((byte)data);
                }

                Thread.Sleep(10); // TODO: Tune this.
            }

            this.logger.LogDebug("Inbound thread for port {0} exiting.", _port.PortName);
            _inboundRunning = false;
        }

        private void OutboundLoop()
        {
            this.logger.LogDebug("Outbound thread for port {0} starting.", _port.PortName);
            _inboundRunning = true;

            while (this.cancellationToken.IsCancellationRequested == false && _inboundRunning)
            {
                while (this.cancellationToken.IsCancellationRequested == false && _inboundRunning && this.DequeueOutgoing(out SimpitStream outbound))
                {
                    byte[] data = outbound.ReadAll(out int offset, out int count);
                    _port.Write(data, offset, count);
                }

                Thread.Sleep(10); // TODO: Tune this.
            }

            this.logger.LogDebug("Outbound thread for port {0} exiting.", _port.PortName);
            _inboundRunning = false;
        }

        public override string ToString()
        {
            return $"{nameof(SerialPeer)}.{_port.PortName}";
        }
    }
}
