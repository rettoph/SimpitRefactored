using KerbalSimpit.Core.Enums;
using System.Collections.Generic;

namespace KerbalSimpit.Core.Configuration
{
    public interface ISimpitConfiguration
    {
        string Documentation { get; }
        int RefreshRate { get; }
        SimpitLogLevelEnum LogLevel { get; }

        IEnumerable<ISerialConfiguration> Serial { get; }
        IEnumerable<ICustomResourceMessageConfiguration> CustomResourceMessages { get; }
    }
}
