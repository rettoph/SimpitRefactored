using SimpitRefactored.Common.Core.Enums;
using SimpitRefactored.Common.Core.Utilities;
using SimpitRefactored.Core.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SimpitRefactored.Unity.Common
{
    public class SimpitConfiguration : ISimpitConfiguration
    {
        public class SerialPeerConfiguration : ISerialPeerConfiguration
        {
            [Persistent]
            public string PortName = string.Empty;
            [Persistent]
            public int BaudRate = 115200;

            string ISerialPeerConfiguration.PortName => this.PortName;

            int ISerialPeerConfiguration.BaudRate => this.BaudRate;
        }

        public class TcpPeerConfiguration : ITcpPeerConfiguration
        {
            [Persistent]
            public int Port;

            int ITcpPeerConfiguration.Port => this.Port;
        }

        public class CustomResourceMessageConfiguration : ICustomResourceMessageConfiguration
        {
            [Persistent]
            public string ResourceName1 = string.Empty;
            [Persistent]
            public string ResourceName2 = string.Empty;
            [Persistent]
            public string ResourceName3 = string.Empty;
            [Persistent]
            public string ResourceName4 = string.Empty;

            string ICustomResourceMessageConfiguration.ResourceName1 => this.ResourceName1;

            string ICustomResourceMessageConfiguration.ResourceName2 => this.ResourceName2;

            string ICustomResourceMessageConfiguration.ResourceName3 => this.ResourceName3;

            string ICustomResourceMessageConfiguration.ResourceName4 => this.ResourceName4;
        }

        private SimpitConfiguration()
        {

        }

        [Persistent]
        public string Documentation = Constants.DocumentationUrl;

        [Persistent]
        public int RefreshRate = 125;

        [Persistent]
        public SimpitLogLevelEnum LogLevel = SimpitLogLevelEnum.Information;

        public bool Verbose = false;

        public List<SerialPeerConfiguration> SerialPeers = new List<SerialPeerConfiguration>();
        public List<TcpPeerConfiguration> TcpPeers = new List<TcpPeerConfiguration>();
        public List<CustomResourceMessageConfiguration> CustomResourceMessages = new List<CustomResourceMessageConfiguration>();

        string ISimpitConfiguration.Documentation => this.Documentation;

        int ISimpitConfiguration.RefreshRate => this.RefreshRate;

        SimpitLogLevelEnum ISimpitConfiguration.LogLevel => this.Verbose ? SimpitLogLevelEnum.Verbose : this.LogLevel;

        IEnumerable<ISerialPeerConfiguration> ISimpitConfiguration.SerialPeers => this.SerialPeers;

        IEnumerable<ITcpPeerConfiguration> ISimpitConfiguration.TcpPeers => this.TcpPeers;

        IEnumerable<ICustomResourceMessageConfiguration> ISimpitConfiguration.CustomResourceMessages => this.CustomResourceMessages;

        private static readonly string FullConfigFilePath;
        public static readonly SimpitConfiguration Instance;

        static SimpitConfiguration()
        {
            SimpitConfiguration.FullConfigFilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), Constants.SettingsFileName).Replace("\\", "/");
            SimpitConfiguration.Instance = SimpitConfiguration.LoadConfiguration();
        }

        public bool TrySave()
        {
            try
            {
                // Rewrite Documentation parameter on every run,
                // so we know it's always pointing at what should
                // be the right place.
                this.Documentation = Constants.DocumentationUrl;

                ConfigNode node = new ConfigNode(Constants.SettingsNodeName);
                node = ConfigNode.CreateConfigFromObject(this, node);

                for (int i = 0; i < this.SerialPeers.Count; i++)
                {
                    ConfigNode portNode = new ConfigNode("Serial");
                    portNode = ConfigNode.CreateConfigFromObject(this.SerialPeers[i], portNode);
                    node.AddNode(portNode);
                }

                for (int i = 0; i < this.TcpPeers.Count; i++)
                {
                    ConfigNode tcpNode = new ConfigNode("Tcp");
                    tcpNode = ConfigNode.CreateConfigFromObject(this.TcpPeers[i], tcpNode);
                    node.AddNode(tcpNode);
                }

                for (int i = 0; i < CustomResourceMessages.Count; i++)
                {
                    ConfigNode resourceNode = new ConfigNode("CustomResourceMessages");
                    resourceNode = ConfigNode.CreateConfigFromObject(CustomResourceMessages[i], resourceNode);
                    node.AddNode(resourceNode);
                }

                ConfigNode wrapper = new ConfigNode(Constants.SettingsNodeName);
                wrapper.AddNode(node);

                Directory.CreateDirectory(Path.GetDirectoryName(SimpitConfiguration.FullConfigFilePath));
                wrapper.Save(SimpitConfiguration.FullConfigFilePath);
                return true;
            }
            catch (Exception ex)
            {
                SimpitLogger.Instance.LogError(ex, "{0}::{1} - Exception", nameof(SimpitConfiguration), nameof(TrySave));
            }

            return false;
        }

        private static SimpitConfiguration LoadConfiguration()
        {
            if (File.Exists(SimpitConfiguration.FullConfigFilePath))
            {
                try
                {
                    SimpitConfiguration instance = new SimpitConfiguration();

                    ConfigNode node = ConfigNode.Load(SimpitConfiguration.FullConfigFilePath);
                    ConfigNode config = node.GetNode(Constants.SettingsNodeName);
                    ConfigNode.LoadObjectFromConfig(instance, config);

                    ConfigNode[] serialNodes = config.GetNodes("SerialPort").Concat(config.GetNodes("Serial")).ToArray();
                    for (int i = 0; i < serialNodes.Length; i++)
                    {
                        SimpitConfiguration.SerialPeerConfiguration portNode = new SimpitConfiguration.SerialPeerConfiguration();
                        ConfigNode.LoadObjectFromConfig(portNode, serialNodes[i]);
                        instance.SerialPeers.Add(portNode);
                    }

                    ConfigNode[] tcpNodes = config.GetNodes("Tcp").ToArray();
                    for (int i = 0; i < tcpNodes.Length; i++)
                    {
                        SimpitConfiguration.TcpPeerConfiguration tcpNode = new SimpitConfiguration.TcpPeerConfiguration();
                        ConfigNode.LoadObjectFromConfig(tcpNode, tcpNodes[i]);
                        instance.TcpPeers.Add(tcpNode);
                    }

                    ConfigNode[] customResources = config.GetNodes("CustomResourceMessages");
                    for (int i = 0; i < customResources.Length; i++)
                    {
                        SimpitConfiguration.CustomResourceMessageConfiguration customResource = new SimpitConfiguration.CustomResourceMessageConfiguration();
                        ConfigNode.LoadObjectFromConfig(customResource, customResources[i]);
                        instance.CustomResourceMessages.Add(customResource);
                    }

                    return instance;
                }
                catch (Exception ex)
                {
                    SimpitLogger.Instance.LogError(ex, "{0}::{1} - Exception", nameof(SimpitConfiguration), nameof(LoadConfiguration));
                    return SimpitConfiguration.GetDefaultConfiguration();
                }
            }
            else
            {
                SimpitConfiguration instance = SimpitConfiguration.GetDefaultConfiguration();
                instance.TrySave();

                return instance;
            }

        }

        private static SimpitConfiguration GetDefaultConfiguration()
        {
            SimpitConfiguration instance = new SimpitConfiguration();

            instance.SerialPeers.Add(new SimpitConfiguration.SerialPeerConfiguration());
            instance.CustomResourceMessages.Add(new SimpitConfiguration.CustomResourceMessageConfiguration());
            instance.CustomResourceMessages.Add(new SimpitConfiguration.CustomResourceMessageConfiguration());

            return instance;
        }
    }
}
