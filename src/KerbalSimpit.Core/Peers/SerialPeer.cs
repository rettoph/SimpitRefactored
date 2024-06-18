using KerbalSimpit.Common.Core;
using System.IO.Ports;

namespace KerbalSimpit.Core.Peers
{
    public class SerialPeer : SimpitPeer
    {
        private readonly SerialPort _port;

        public override bool Running => base.Running && _port.IsOpen;

        public SerialPeer(string name, int baudRate) : base(name)
        {
            _port = new SerialPort(name, baudRate, Parity.None, 8, StopBits.One);


            // To allow communication from a Pi Pico, the DTR seems to be mandatory to allow the connection
            // This does not seem to prevent communication from Arduino.
            _port.DtrEnable = true;
        }

        protected override bool TryOpen()
        {
            _port.Open();

            return true;
        }

        protected override bool TryClose()
        {
            _port.Close();

            return true;
        }

        protected override bool TryReadByte(out byte value)
        {
            if (_port.BytesToRead == 0)
            {
                value = default;
                return false;
            }

            int readResult = _port.ReadByte();
            if (readResult == Constants.EndOfData)
            { // This should not happen thanks to the BytesToRead check above
                value = default;
                return false;
            }

            value = (byte)readResult;
            return true;
        }

        protected override void WriteBytes(byte[] data, int offset, int length)
        {
            _port.Write(data, offset, length);
        }
    }
}
