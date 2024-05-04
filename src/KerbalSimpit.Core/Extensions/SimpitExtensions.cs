using KerbalSimpit.Core.Peers;

namespace KerbalSimpit.Core.Extensions
{
    public static class SimpitExtensions
    {
        public static Simpit AddSerial(this Simpit simpit, string name, int baudRate)
        {
            return simpit.Add(new SerialPeer(name, baudRate));
        }
    }
}
