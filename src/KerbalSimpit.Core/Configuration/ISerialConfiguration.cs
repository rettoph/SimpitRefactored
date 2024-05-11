namespace KerbalSimpit.Core.Configuration
{
    public interface ISerialConfiguration
    {
        string PortName { get; }
        int BaudRate { get; }
    }
}
