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

        public Logger()
        {
            InitializeComponent();
        }

        public void Log(SimpitLogLevelEnum level, string template, object[] args)
        {
            this.AddMessage(new SolidColorBrush(Color.FromRgb(255, 0, 0)), $"[{level}] {string.Format(template, args)}");
        }

        public void Log(SimpitLogLevelEnum level, Exception ex, string template, object[] args)
        {
            this.AddMessage(new SolidColorBrush(Color.FromRgb(255, 0, 0)), $"[{level}] {string.Format(template, args)}\n{ex}");
        }

        private void AddMessage(Brush background, string message)
        {
            this.Dispatcher.Invoke(() =>
            {
                TextBlock text = new TextBlock()
                {
                    Text = message,
                    Background = background
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
