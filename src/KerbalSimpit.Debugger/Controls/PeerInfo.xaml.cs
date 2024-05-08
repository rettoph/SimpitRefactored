using KerbalSimpit.Core;
using KerbalSimpit.Core.Enums;
using KerbalSimpit.Core.Peers;
using System.Runtime.InteropServices;
using System.Windows.Controls;

namespace KerbalSimpit.Debugger.Controls
{
    /// <summary>
    /// Interaction logic for PeerInfo.xaml
    /// </summary>
    public partial class PeerInfo : UserControl, IDisposable
    {
        private class MessageTypeCount
        {
            private readonly TextBlock _textBlock;
            private readonly SimpitMessageType _type;
            private int _value;

            public TextBlock Label => _textBlock;
            public int Value => _value;

            public MessageTypeCount(SimpitMessageType type)
            {
                _type = type;
                _textBlock = new TextBlock()
                {
                    Margin = new System.Windows.Thickness(15, 2, 5, 2)
                };
                _value = 0;

                this.Clean();
            }

            public void Increment()
            {
                _value++;
                this.Clean();
            }

            private void Clean()
            {
                _textBlock.Text = $"{_type} {(_type.Type == SimputMessageTypeEnum.Outgoing ? "=>" : "<=")} {_value.ToString("#,###,##0")}";
            }

            public void Reset()
            {
                _value = 0;
                this.Clean();
            }
        }

        private readonly SimpitPeer _peer;
        private Dictionary<SimpitMessageType, TextBlock> _outgoingSubscriptions;
        private Dictionary<SimpitMessageType, MessageTypeCount> _cache;
        private int _incomingCount;
        private int _outgoingCount;

        public PeerInfo(SimpitPeer peer)
        {
            InitializeComponent();

            _peer = peer;
            _outgoingSubscriptions = new Dictionary<SimpitMessageType, TextBlock>();
            _cache = new Dictionary<SimpitMessageType, MessageTypeCount>();

            _peer.OnStatusChanged += this.HandleStatusChanged;
            _peer.OnOutgoingSubscribed += this.HandleOutgoingSubscribed;
            _peer.OnOutgoingUnsubscribed += this.HandleOutgoingUsubscribed;
            _peer.OnIncomingMessage += this.HandleMessage;
            _peer.OnOutgoingMessage += this.HandleMessage;

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
            this.MessagesLabel.Content = $"Messages (I: {_incomingCount.ToString("#,###,##0")}, O: {_outgoingCount.ToString("#,###,##0")})";
        }

        private void HandleStatusChanged(object? sender, ConnectionStatusEnum e)
        {
            this.Dispatcher.Invoke(() =>
            {
                foreach (MessageTypeCount count in _cache.Values)
                {
                    count.Reset();
                }

                this.Clean();
            });

        }

        private void HandleOutgoingSubscribed(object? sender, SimpitMessageType e)
        {
            this.Dispatcher.Invoke(() =>
            {
                TextBlock text = new TextBlock();
                text.Margin = new System.Windows.Thickness(15, 2, 5, 2);
                text.Text = e.ToString();
                _outgoingSubscriptions.Add(e, text);
                this.OutgoingSubscriptionsContainer.Children.Add(text);

                this.Clean();
            });
        }

        private void HandleOutgoingUsubscribed(object? sender, SimpitMessageType e)
        {
            if (_outgoingSubscriptions.Remove(e, out TextBlock? text))
            {
                this.Dispatcher.Invoke(() =>
                {
                    this.OutgoingSubscriptionsContainer.Children.Remove(text);

                    this.Clean();
                });
            }
        }

        private void HandleMessage(object? sender, ISimpitMessage e)
        {
            this.Dispatcher.Invoke(() =>
            {
                ref MessageTypeCount? count = ref CollectionsMarshal.GetValueRefOrAddDefault(_cache, e.Type, out bool exists);
                if (exists == false)
                {
                    count = new MessageTypeCount(e.Type);
                }
                else
                {
                    this.MessagesContainer.Children.Remove(count!.Label);
                }

                if (e.Type.Type == SimputMessageTypeEnum.Outgoing)
                {
                    _outgoingCount++;
                }
                else if (e.Type.Type == SimputMessageTypeEnum.Incoming)
                {
                    _incomingCount++;
                }

                count!.Increment();

                this.Clean();

                this.MessagesContainer.Children.Clear();
                foreach (MessageTypeCount item in _cache.Values.OrderByDescending(x => x.Value))
                {
                    this.MessagesContainer.Children.Add(item.Label);
                }
            });
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
