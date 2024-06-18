using KerbalSimpit.Common.Core.Enums;
using KerbalSimpit.Core.Configuration;

namespace KerbalSimpit.Debugger
{
    public class SimpitConfiguration : ISimpitConfiguration
    {
        public class SerialPeerConfiguration : ISerialPeerConfiguration
        {
            public string PortName { get; set; } = string.Empty;
            public int BaudRate { get; set; } = 115200;
        }

        public class TcpPeerConfiguration : ITcpPeerConfiguration
        {
            public string Host { get; set; } = string.Empty;
            public int Port { get; set; } = 1337;
        }

        public class CustomResourceMessageConfiguration : ICustomResourceMessageConfiguration
        {
            public string ResourceName1 { get; set; } = string.Empty;
            public string ResourceName2 { get; set; } = string.Empty;
            public string ResourceName3 { get; set; } = string.Empty;
            public string ResourceName4 { get; set; } = string.Empty;
        }

        public string Documentation { get; set; } = string.Empty;
        public bool Verbose { get; set; } = false;
        public int RefreshRate { get; set; } = 128;
        public SimpitLogLevelEnum LogLevel { get; set; } = SimpitLogLevelEnum.Information;

        public List<SerialPeerConfiguration> SerialPeers { get; set; } = new List<SerialPeerConfiguration>();
        public List<TcpPeerConfiguration> TcpPeers { get; set; } = new List<TcpPeerConfiguration>();
        public List<CustomResourceMessageConfiguration> CustomResourceMessages { get; set; } = new List<CustomResourceMessageConfiguration>();

        SimpitLogLevelEnum ISimpitConfiguration.LogLevel => this.LogLevel;

        IEnumerable<ISerialPeerConfiguration> ISimpitConfiguration.SerialPeers => this.SerialPeers;

        IEnumerable<ITcpPeerConfiguration> ISimpitConfiguration.TcpPeers => this.TcpPeers;

        IEnumerable<ICustomResourceMessageConfiguration> ISimpitConfiguration.CustomResourceMessages => this.CustomResourceMessages;
    }
}
