using KerbalSimpit.Core.Enums;
using KerbalSimpit.Core.Utilities;
using KerbalSimpit.Unity.Common;
using KSP.UI.Screens;
using UnityEngine;

namespace KerbalSimpit.Unity.KSP1.SimpitGUI
{
    // Start at main menu
    [KSPAddon(KSPAddon.Startup.MainMenu, true)]
    public class AppButton : SimpitBehaviour
    {
        const ApplicationLauncher.AppScenes buttonScenes = ApplicationLauncher.AppScenes.SPACECENTER | ApplicationLauncher.AppScenes.FLIGHT | ApplicationLauncher.AppScenes.MAPVIEW;
        private static ApplicationLauncherButton button;

        private static Texture2D iconRed, iconOrange, iconGreen;

        public static Callback Toggle = delegate { };

        static bool buttonVisible
        {
            get
            {
                return true;
            }
        }

        public void UpdateVisibility()
        {
            if (button != null)
            {
                button.VisibleInScenes = buttonVisible ? buttonScenes : 0;
            }
        }

        private static void onToggle()
        {
            Toggle();
        }

        public override void Start()
        {
            base.Start();

            iconRed = GameDatabase.Instance.GetTexture("KerbalSimpit/Simpit_icon_red", false);
            iconOrange = GameDatabase.Instance.GetTexture("KerbalSimpit/Simpit_icon_orange", false);
            iconGreen = GameDatabase.Instance.GetTexture("KerbalSimpit/Simpit_icon_green", false);

            GameObject.DontDestroyOnLoad(this);
            GameEvents.onGUIApplicationLauncherReady.Add(OnGUIAppLauncherReady);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            GameEvents.onGUIApplicationLauncherReady.Remove(OnGUIAppLauncherReady);
        }

        void OnGUIAppLauncherReady()
        {
            if (ApplicationLauncher.Ready && button == null)
            {
                button = ApplicationLauncher.Instance.AddModApplication(onToggle, onToggle, null, null, null, null, ApplicationLauncher.AppScenes.ALWAYS, iconRed);
                UpdateVisibility();
            }
        }

        void Update()
        {
            if (button == null) return; // button not yet initialised ?

            if (this.simpit.Peers.Count == 0)
            {
                button.SetTexture(iconRed);
                return;
            }

            ConnectionStatusEnum status = this.simpit.Peers[0].Status;
            if (status == ConnectionStatusEnum.CLOSED || status == ConnectionStatusEnum.ERROR)
            {
                button.SetTexture(iconRed);
                return;
            }

            if (status == ConnectionStatusEnum.WAITING_HANDSHAKE || status == ConnectionStatusEnum.HANDSHAKE)
            {
                button.SetTexture(iconOrange);
                return;
            }

            if (status == ConnectionStatusEnum.CONNECTED || status == ConnectionStatusEnum.IDLE)
            {
                button.SetTexture(iconGreen);
                return;
            }

            //All cases should be covered, this should not happen.
            button.SetTexture(iconRed);
            this.logger.LogWarning("{0}::{1} - Unreachable state detected {2}", nameof(AppButton), nameof(Update), status);
        }
    }


    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class Toolbar_Flight : MonoBehaviour
    {
        public void Awake()
        {
            AppButton.Toggle += WindowFlight.ToggleGUI;
        }

        void OnDestroy()
        {
            AppButton.Toggle -= WindowFlight.ToggleGUI;
        }
    }

    [KSPAddon(KSPAddon.Startup.SpaceCentre, false)]
    public class Toolbar_SpaceCenter : MonoBehaviour
    {
        public void Awake()
        {
            AppButton.Toggle += WindowSpaceCenter.ToggleGUI;
        }

        void OnDestroy()
        {
            AppButton.Toggle -= WindowSpaceCenter.ToggleGUI;
        }
    }
}
