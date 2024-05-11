using System.Windows.Controls;
using System.Windows.Media;

namespace KerbalSimpit.Debugger.Controls
{
    /// <summary>
    /// Interaction logic for SimpleTextSubscriberControl.xaml
    /// </summary>
    public partial class SimpleTextSubscriberControl : UserControl
    {
        private DateTime _updatedAt;
        private Brush _idleBrush;
        private Brush _activeBrush;

        public SimpleTextSubscriberControl(string title)
        {
            InitializeComponent();

            this.Title.Content = title;

            _idleBrush = this.Border.BorderBrush;
            _activeBrush = new SolidColorBrush(Color.FromRgb(50, 175, 50));
        }

        public void SetValue(string value)
        {
            this.Dispatcher.Invoke(() =>
            {
                this.Value.Content = value;
                this.Border.BorderBrush = _activeBrush;
            });
            _updatedAt = DateTime.Now;

            Task.Run(async () =>
            {
                await Task.Delay(250);

                if (DateTime.Now - _updatedAt > TimeSpan.FromMilliseconds(200))
                {
                    this.Dispatcher.Invoke(() => this.Border.BorderBrush = _idleBrush);
                }
            });
        }
    }
}
