using KerbalSimpit.Core;
using KerbalSimpit.Core.KSP.Messages;
using KerbalSimpit.Core.Peers;
using KerbalSimpit.Unity.Common;

namespace KerbalSimpit.Unity.KSP1.Controllers
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class NavBallController : SimpitBehaviour,
        ISimpitMessageSubscriber<NavBall.NavballMode>
    {
        public void Process(SimpitPeer peer, ISimpitMessage<NavBall.NavballMode> message)
        {
            FlightGlobals.CycleSpeedModes();
        }
    }
}
