using KerbalSimpit.Core;
using KerbalSimpit.Core.Enums;
using KerbalSimpit.Core.Peers;
using System.Windows.Controls;

namespace KerbalSimpit.Debugger.Controls
{
    /// <summary>
    /// Interaction logic for PeerInfo.xaml
    /// </summary>
    public partial class PeerInfo : UserControl, IDisposable
    {
        private readonly SimpitPeer _peer;
        private Dictionary<SimpitMessageType, TextBlock> _outgoingSubscriptions;

        public PeerInfo(SimpitPeer peer)
        {
            InitializeComponent();

            _peer = peer;
            _outgoingSubscriptions = new Dictionary<SimpitMessageType, TextBlock>();

            _peer.OnStatusChanged += this.HandleStatusChanged;
            _peer.OnOutgoingSubscribed += this.HandleOutgoingSubscribed;
            _peer.OnOutgoingUnsubscribed += this.HandleOutgoingUsubscribed;

            this.Clean();
        }

        public void Dispose()
        {
            _peer.OnStatusChanged -= this.HandleStatusChanged;
        }

        private void Clean()
        {
            this.Label.Content = $"{_peer}: {_peer.Status}";
            this.Toggle.Content = _peer.Status == ConnectionStatusEnum.CLOSED ? "Open" : "Close";

            this.OutgoingSubscriptionsLabel.Content = $"Outgoing Subscriptions ({_peer.OutgoingSubscriptions.Count()}):";
        }

        private void HandleStatusChanged(object? sender, ConnectionStatusEnum e)
        {
            this.Clean();
        }

        private void HandleOutgoingSubscribed(object? sender, SimpitMessageType e)
        {
            TextBlock text = new TextBlock();
            text.Margin = new System.Windows.Thickness(15, 2, 5, 2);
            text.Text = e.ToString();
            _outgoingSubscriptions.Add(e, text);
            this.OutgoingSubscriptionsContainer.Children.Add(text);

            this.Clean();
        }

        private void HandleOutgoingUsubscribed(object? sender, SimpitMessageType e)
        {
            if (_outgoingSubscriptions.Remove(e, out TextBlock? text))
            {
                this.OutgoingSubscriptionsContainer.Children.Remove(text);
            }

            this.Clean();
        }

        private void Toggle_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (_peer.Status == ConnectionStatusEnum.CLOSED)
            {
                _peer.Open();
            }
            else
            {
                _peer.Close();
            }

            this.Clean();
        }
    }
}
