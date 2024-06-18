namespace KerbalSimpit.Core.Configuration
{
    public interface ISerialPeerConfiguration
    {
        string PortName { get; }
        int BaudRate { get; }
    }
}
