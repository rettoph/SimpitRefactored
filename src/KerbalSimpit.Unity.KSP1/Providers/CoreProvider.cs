using KerbalSimpit.Core;
using KerbalSimpit.Core.Messages;
using KerbalSimpit.Core.Peers;
using KerbalSimpit.Core.Utilities;
using KerbalSimpit.Unity.Common;

namespace KerbalSimpit.Unity.KSP1.Providers
{
    [KSPAddon(KSPAddon.Startup.Instantly, true)]
    public class CoreProvider : SimpitBehaviour,
        ISimpitMessageSubscriber<EchoRequest>,
        ISimpitMessageSubscriber<EchoResponse>,
        ISimpitMessageSubscriber<CustomLog>
    {
        public void Start()
        {
            DontDestroyOnLoad(this); // Make this provider persistent

            this.simpit.AddIncomingSubscriber<EchoRequest>(this);
            this.simpit.AddIncomingSubscriber<EchoResponse>(this);
            this.simpit.AddIncomingSubscriber<CustomLog>(this);
        }

        public void OnDestroy()
        {
            this.simpit.RemoveIncomingSubscriber<EchoRequest>(this);
            this.simpit.RemoveIncomingSubscriber<EchoResponse>(this);
            this.simpit.RemoveIncomingSubscriber<CustomLog>(this);
        }

        void ISimpitMessageSubscriber<EchoRequest>.Process(SimpitPeer peer, ISimpitMessage<EchoRequest> message)
        {
            this.logger.LogVerbose("Echo request on peer {0}. Replying.", peer);
            peer.EnqueueOutgoing(new EchoResponse()
            {
                Data = message.Data.Data
            });
        }

        void ISimpitMessageSubscriber<EchoResponse>.Process(SimpitPeer peer, ISimpitMessage<EchoResponse> message)
        {
            this.logger.LogVerbose("Echo reply received on port {0}", peer);
        }

        void ISimpitMessageSubscriber<CustomLog>.Process(SimpitPeer peer, ISimpitMessage<CustomLog> message)
        {
            if (message.Data.Flags.HasFlag(CustomLog.FlagsEnum.Verbose))
            {
                this.logger.LogVerbose(message.Data.Value);
            }

            if (message.Data.Flags.HasFlag(CustomLog.FlagsEnum.PrintToScreen))
            {
                ScreenMessages.PostScreenMessage(message.Data.Value);
            }
        }
    }
}
