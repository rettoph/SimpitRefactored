using KerbalSimpit.Core;
using KerbalSimpit.Core.KSP.Enums;
using KerbalSimpit.Core.Peers;
using KerbalSimpit.Core.Utilities;
using KSP.UI.Screens;
using System.Collections.Generic;
using VesselMessages = KerbalSimpit.Core.KSP.Messages.Vessel;

namespace KerbalSimpit.Unity.KSP1.Controllers
{
    public partial class VesselController : ISimpitMessageSubscriber<VesselMessages.Incoming.CustomActionGroupEnable>,
        ISimpitMessageSubscriber<VesselMessages.Incoming.CustomActionGroupDisable>,
        ISimpitMessageSubscriber<VesselMessages.Incoming.CustomActionGroupToggle>,
        ISimpitMessageSubscriber<VesselMessages.Incoming.ActionGroupActivate>,
        ISimpitMessageSubscriber<VesselMessages.Incoming.ActionGroupDeactivate>,
        ISimpitMessageSubscriber<VesselMessages.Incoming.ActionGroupToggle>
    {
        public void Process(SimpitPeer peer, ISimpitMessage<VesselMessages.Incoming.CustomActionGroupEnable> message)
        {
            foreach (int idx in message.Data.GroupIds)
            {
                FlightGlobals.ActiveVessel.ActionGroups.SetGroup(this.ActionGroupIDs[idx], true);
            }
        }

        public void Process(SimpitPeer peer, ISimpitMessage<VesselMessages.Incoming.CustomActionGroupDisable> message)
        {
            foreach (int idx in message.Data.GroupIds)
            {
                FlightGlobals.ActiveVessel.ActionGroups.SetGroup(this.ActionGroupIDs[idx], false);
            }
        }

        public void Process(SimpitPeer peer, ISimpitMessage<VesselMessages.Incoming.CustomActionGroupToggle> message)
        {
            foreach (int idx in message.Data.GroupIds)
            {
                FlightGlobals.ActiveVessel.ActionGroups.ToggleGroup(this.ActionGroupIDs[idx]);
            }
        }

        public void Process(SimpitPeer peer, ISimpitMessage<VesselMessages.Incoming.ActionGroupActivate> message)
        {
            foreach (KSPActionGroup group in this.GetActionGroups(message.Data.Flags))
            {
                this.logger.LogInformation("Activating {0}", group);
                FlightGlobals.ActiveVessel.ActionGroups.SetGroup(group, true);

                if (group == KSPActionGroup.Stage)
                {
                    StageManager.ActivateNextStage();
                }
            }
        }

        public void Process(SimpitPeer peer, ISimpitMessage<VesselMessages.Incoming.ActionGroupDeactivate> message)
        {
            foreach (KSPActionGroup group in this.GetActionGroups(message.Data.Flags))
            {
                this.logger.LogInformation("Deactivating {0}", group);
                FlightGlobals.ActiveVessel.ActionGroups.SetGroup(group, false);
            }
        }

        public void Process(SimpitPeer peer, ISimpitMessage<VesselMessages.Incoming.ActionGroupToggle> message)
        {
            foreach (KSPActionGroup group in this.GetActionGroups(message.Data.Flags))
            {
                this.logger.LogInformation("Toggling {0}", group);
                FlightGlobals.ActiveVessel.ActionGroups.ToggleGroup(group);

                if (group == KSPActionGroup.Stage)
                {
                    StageManager.ActivateNextStage();
                }
            }
        }

        private IEnumerable<KSPActionGroup> GetActionGroups(ActionGroupFlags flgas)
        {
            if (flgas.HasFlag(ActionGroupFlags.Stage))
            {
                yield return KSPActionGroup.Stage;
            }

            if (flgas.HasFlag(ActionGroupFlags.Gear))
            {
                yield return KSPActionGroup.Gear;
            }

            if (flgas.HasFlag(ActionGroupFlags.Light))
            {
                yield return KSPActionGroup.Light;
            }

            if (flgas.HasFlag(ActionGroupFlags.RCS))
            {
                yield return KSPActionGroup.RCS;
            }

            if (flgas.HasFlag(ActionGroupFlags.SAS))
            {
                yield return KSPActionGroup.SAS;
            }

            if (flgas.HasFlag(ActionGroupFlags.Brakes))
            {
                yield return KSPActionGroup.Brakes;
            }

            if (flgas.HasFlag(ActionGroupFlags.Abort))
            {
                yield return KSPActionGroup.Abort;
            }
        }
    }
}
