using KerbalSimpit.Common.Core.Enums;
using System.Collections.Generic;

namespace KerbalSimpit.Core.Configuration
{
    public interface ISimpitConfiguration
    {
        string Documentation { get; }
        int RefreshRate { get; }
        SimpitLogLevelEnum LogLevel { get; }

        IEnumerable<ISerialPeerConfiguration> SerialPeers { get; }
        IEnumerable<ITcpPeerConfiguration> TcpPeers { get; }
        IEnumerable<ICustomResourceMessageConfiguration> CustomResourceMessages { get; }
    }
}
