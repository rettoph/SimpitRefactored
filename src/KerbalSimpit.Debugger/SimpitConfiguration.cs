namespace KerbalSimpit.Debugger
{
    public class SimpitConfiguration
    {
        public class SerialConfiguration
        {
            public string Name { get; set; } = string.Empty;
            public int BaudRate { get; set; } = 115200;
        }

        public SerialConfiguration[] Serial { get; set; } = Array.Empty<SerialConfiguration>();
    }
}
