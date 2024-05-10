using KerbalSimpit.Core.Configuration;
using KerbalSimpit.Core.Enums;

namespace KerbalSimpit.Debugger
{
    public class SimpitConfiguration : ISimpitConfiguration
    {
        public class SerialConfiguration : ISerialConfiguration
        {
            public string PortName { get; set; } = string.Empty;
            public int BaudRate { get; set; } = 115200;
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
        public int RefreshRate { get; set; } = 125;
        public SimpitLogLevelEnum LogLevel { get; set; } = SimpitLogLevelEnum.Information;

        public List<SerialConfiguration> Serial { get; set; } = new List<SerialConfiguration>();
        public List<CustomResourceMessageConfiguration> CustomResourceMessages { get; set; } = new List<CustomResourceMessageConfiguration>();

        SimpitLogLevelEnum ISimpitConfiguration.LogLevel => this.LogLevel;

        IEnumerable<ISerialConfiguration> ISimpitConfiguration.Serial => this.Serial;

        IEnumerable<ICustomResourceMessageConfiguration> ISimpitConfiguration.CustomResourceMessages => this.CustomResourceMessages;
    }
}
