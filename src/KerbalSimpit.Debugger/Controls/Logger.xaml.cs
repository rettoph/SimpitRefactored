using KerbalSimpit.Core;
using KerbalSimpit.Core.Enums;
using KerbalSimpit.Core.Messages;
using KerbalSimpit.Core.Peers;
using KerbalSimpit.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KerbalSimpit.Debugger.Controls
{
    /// <summary>
    /// Interaction logic for LogControl.xaml
    /// </summary>
    public partial class Logger : UserControl, ISimpitLogger, ISimpitMessageConsumer<CustomLog>
    {
        public SimpitLogLevelEnum LogLevel { get; set; }

        private Dictionary<SimpitLogLevelEnum, Brush> _colors;

        public Logger()
        {
            InitializeComponent();

            this.LogLevel = SimpitLogLevelEnum.Verbose;
            _colors = new Dictionary<SimpitLogLevelEnum, Brush>()
            {
                [SimpitLogLevelEnum.Error] = new SolidColorBrush(Color.FromRgb(255, 0, 0)),
                [SimpitLogLevelEnum.Warning] = new SolidColorBrush(Color.FromRgb(255, 255, 0)),
                [SimpitLogLevelEnum.Information] = new SolidColorBrush(Color.FromRgb(255, 255, 255)),
                [SimpitLogLevelEnum.Debug] = new SolidColorBrush(Color.FromRgb(0, 255, 255)),
                [SimpitLogLevelEnum.Verbose] = new SolidColorBrush(Color.FromRgb(255, 0, 255))
            };
        }

        public void Log(SimpitLogLevelEnum level, string template, object[] args)
        {
            this.AddMessage(_colors[level], $"[{level}] {string.Format(template, args)}");
        }

        public void Log(SimpitLogLevelEnum level, Exception ex, string template, object[] args)
        {
            this.AddMessage(_colors[level], $"[{level}] {string.Format(template, args)}\n{ex}");
        }

        private void AddMessage(Brush color, string message)
        {
            this.Dispatcher.Invoke(() =>
            {
                TextBlock text = new TextBlock()
                {
                    Text = message,
                    Foreground = color,
                    FontWeight = FontWeights.Bold,
                };


                this.MessageContainer.Children.Add(text);
                this.ScrollContainer.ScrollToBottom();
            });
        }

        public void Consume(SimpitPeer peer, ISimpitMessage<CustomLog> message)
        {
            this.LogInformation(message.Content.Value);
        }
    }
}
