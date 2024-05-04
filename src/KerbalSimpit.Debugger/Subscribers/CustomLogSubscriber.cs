using KerbalSimpit.Core;
using KerbalSimpit.Core.Messages;
using KerbalSimpit.Core.Peers;
using KerbalSimpit.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KerbalSimpit.Debugger.Subscribers
{
    internal class CustomLogSubscriber : ISimpitMessageSubscriber<CustomLog>
    {
        private readonly ISimpitLogger _logger;

        public CustomLogSubscriber(ISimpitLogger logger)
        {
            _logger = logger;
        }

        public void Process(SimpitPeer peer, ISimpitMessage<CustomLog> message)
        {
            _logger.LogInformation("{0} - {1}", nameof(CustomLog), message.Content.Value);
        }
    }
}
