using KerbalSimpit.Core;
using KerbalSimpit.Core.Messages;
using KerbalSimpit.Core.Peers;
using KerbalSimpit.Core.Utilities;
using UnityEngine;

namespace KerbalSimpit.Unity.KSP1.Providers
{
    [KSPAddon(KSPAddon.Startup.Instantly, true)]
    public class CoreProvider : MonoBehaviour,
    ISimpitMessageSubscriber<EchoRequest>,
    ISimpitMessageSubscriber<EchoResponse>,
    ISimpitMessageSubscriber<CustomLog>
    {
        public void Start()
        {
            DontDestroyOnLoad(this); // Make this provider persistent

            KerbalSimpitUnityKSP1.Simpit.AddIncomingSubscriber<EchoRequest>(this);
            KerbalSimpitUnityKSP1.Simpit.AddIncomingSubscriber<EchoResponse>(this);
            KerbalSimpitUnityKSP1.Simpit.AddIncomingSubscriber<CustomLog>(this);
        }

        public void OnDestroy()
        {
            KerbalSimpitUnityKSP1.Simpit.RemoveIncomingSubscriber<EchoRequest>(this);
            KerbalSimpitUnityKSP1.Simpit.RemoveIncomingSubscriber<EchoResponse>(this);
            KerbalSimpitUnityKSP1.Simpit.RemoveIncomingSubscriber<CustomLog>(this);
        }

        void ISimpitMessageSubscriber<EchoRequest>.Process(SimpitPeer peer, ISimpitMessage<EchoRequest> message)
        {
            KerbalSimpitUnityKSP1.Logger.LogVerbose("Echo request on peer {0}. Replying.", peer);
            peer.EnqueueOutgoing(new EchoResponse()
            {
                Data = message.Data.Data
            });
        }

        void ISimpitMessageSubscriber<EchoResponse>.Process(SimpitPeer peer, ISimpitMessage<EchoResponse> message)
        {
            KerbalSimpitUnityKSP1.Logger.LogVerbose("Echo reply received on port {0}", peer);
        }

        void ISimpitMessageSubscriber<CustomLog>.Process(SimpitPeer peer, ISimpitMessage<CustomLog> message)
        {
            if (message.Data.Flags.HasFlag(CustomLog.FlagsEnum.Verbose))
            {
                KerbalSimpitUnityKSP1.Logger.LogVerbose(message.Data.Value);
            }

            if (message.Data.Flags.HasFlag(CustomLog.FlagsEnum.PrintToScreen))
            {
                ScreenMessages.PostScreenMessage(message.Data.Value);
            }
        }
    }
}
