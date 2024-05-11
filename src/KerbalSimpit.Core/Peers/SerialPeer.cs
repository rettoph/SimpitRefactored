using KerbalSimpit.Core.Constants;
using KerbalSimpit.Core.Utilities;
using System;
using System.IO.Ports;
using System.Threading;

namespace KerbalSimpit.Core.Peers
{
    public class SerialPeer : SimpitPeer
    {
        private readonly SerialPort _port;
        private Simpit _simpit;
        private Thread _inboundThread;
        private Thread _outboundThread;


        private bool _inboundRunning;
        private bool _outboundRunning;

        public override bool Running => _inboundRunning || _outboundRunning;

        public SerialPeer(string name, int baudRate) : base()
        {
            _port = new SerialPort(name, baudRate, Parity.None, 8, StopBits.One);


            // To allow communication from a Pi Pico, the DTR seems to be mandatory to allow the connection
            // This does not seem to prevent communication from Arduino.
            _port.DtrEnable = true;
        }

        public override void Initialize(Simpit simpit)
        {
            base.Initialize(simpit);

            _simpit = simpit;
        }

        protected override bool TryOpen()
        {
            if (this.Running == true)
            {
                this.logger.LogWarning("{0}::{1} - Already running", nameof(SerialPeer), nameof(TryClose));
                return false;
            }

            try
            {
                _inboundThread = new Thread(this.InboundLoop);
                _outboundThread = new Thread(this.OutboundLoop);

                _port.Open();
                _inboundThread.Start();
                _outboundThread.Start();
                return true;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "{0}::{1} - Exception", nameof(SerialPeer), nameof(TryOpen));
                return false;
            }
        }

        protected override bool TryClose()
        {
            try
            {
                _inboundRunning = false;
                _outboundRunning = false;
                _port.Close();
                return true;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "{0}::{1} - Exception", nameof(SerialPeer), nameof(TryClose));
                return false;
            }
        }

        private void InboundLoop()
        {
            this.logger.LogDebug("Inbound thread for port {0} starting.", _port.PortName);
            _inboundRunning = true;

            while (this.cancellationToken.IsCancellationRequested == false && _inboundRunning && _port.IsOpen)
            {
                try
                {
                    while (_port.BytesToRead > 0 && this.cancellationToken.IsCancellationRequested == false && _inboundRunning)
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
                catch (Exception ex)
                {
                    this.logger.LogError(ex, "{0}::{1} - Exception", nameof(SerialPeer), nameof(InboundLoop));
                }
            }

            this.logger.LogDebug("Inbound thread for port {0} exiting.", _port.PortName);
            this.Close();
        }

        private void OutboundLoop()
        {
            this.logger.LogDebug("Outbound thread for port {0} starting.", _port.PortName);
            _inboundRunning = true;

            while (this.cancellationToken.IsCancellationRequested == false && _inboundRunning && _port.IsOpen)
            {
                try
                {
                    this.EnqueueOutgoingSubscriptions();

                    int enqueuedOutgoingCount = this.GetEnqueuedOutgoingCount();
                    if (enqueuedOutgoingCount == 0)
                    {
                        Thread.Sleep(_simpit.Configuration.RefreshRate);
                        continue;
                    }

                    int refreshSliceRate = _simpit.Configuration.RefreshRate / enqueuedOutgoingCount;
                    while (this.cancellationToken.IsCancellationRequested == false && _inboundRunning && this.DequeueOutgoing(out SimpitStream outbound))
                    {
                        byte[] data = outbound.ReadAll(out int offset, out int count);
                        _port.Write(data, offset, count);
                        Thread.Sleep(refreshSliceRate);
                    }
                }
                catch (Exception ex)
                {
                    this.logger.LogError(ex, "{0}::{1} - Exception", nameof(SerialPeer), nameof(OutboundLoop));
                }
            }

            this.logger.LogDebug("Outbound thread for port {0} exiting.", _port.PortName);
            this.Close();
        }

        public override string ToString()
        {
            return $"{nameof(SerialPeer)}.{_port.PortName}";
        }
    }
}
