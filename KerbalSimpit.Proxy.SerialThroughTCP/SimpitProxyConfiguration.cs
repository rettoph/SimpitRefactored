namespace KerbalSimpit.Proxy.SerialThroughTCP
{
    public sealed class SimpitProxyConfiguration
    {
        public string SerialPortName { get; set; } = string.Empty;
        public int SerialBaudRate { get; set; }

        public string TcpClientHost { get; set; } = string.Empty;
        public int TcpClientPort { get; set; }
    }
}
