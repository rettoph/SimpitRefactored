using KerbalSimpit.Core.Peers;
using KerbalSimpit.Unity.Common;
using UnityEngine;

namespace KerbalSimpit.Unity.KSP1.SimpitGUI
{


    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class WindowFlight : Window
    {

    }

    [KSPAddon(KSPAddon.Startup.SpaceCentre, false)]
    public class WindowSpaceCenter : Window
    {

    }

    public class Window : SimpitBehaviour
    {
        static Rect windowpos;
        private static bool gui_enabled;
        private static bool hide_ui;

        static Window instance;


        public static void ToggleGUI()
        {
            gui_enabled = !gui_enabled;
            if (instance != null)
            {
                instance.UpdateGUIState();
            }
        }

        public static void HideGUI()
        {
            gui_enabled = false;
            if (instance != null)
            {
                instance.UpdateGUIState();
            }
        }

        public static void ShowGUI()
        {
            gui_enabled = true;
            if (instance != null)
            {
                instance.UpdateGUIState();
            }
        }

        void UpdateGUIState()
        {
            enabled = !hide_ui && gui_enabled;
        }

        void onHideUI()
        {
            hide_ui = true;
            UpdateGUIState();
        }

        void onShowUI()
        {
            hide_ui = false;
            UpdateGUIState();
        }

        public void Awake()
        {
            instance = this;
            GameEvents.onHideUI.Add(onHideUI);
            GameEvents.onShowUI.Add(onShowUI);
        }

        public override void Start()
        {
            base.Start();

            UpdateGUIState();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            instance = null;
            GameEvents.onHideUI.Remove(onHideUI);
            GameEvents.onShowUI.Remove(onShowUI);
        }

        void WindowGUI(int windowID)
        {
            GUILayout.BeginVertical();

            foreach (SimpitPeer peer in this.simpit.Peers)
            {
                GUILayout.Label(peer + ": " + peer.Status);

                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Open"))
                {
                    peer.Open();
                }
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Close"))
                {
                    peer.Close();
                }
                GUILayout.EndHorizontal();
            }

            if (this.simpit.Peers.Count > 1)
            {
                //only put the Start all/Close all button if there is several ports
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Open all"))
                {
                    this.simpit.OpenAll();
                }
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Close all"))
                {
                    this.simpit.CloseAll();
                }
                GUILayout.EndHorizontal();
            }

            GUILayout.EndVertical();
            UnityEngine.GUI.DragWindow(new Rect(0, 0, 1000, 20));
        }

        void OnGUI()
        {
            if (gui_enabled)
            {
                UnityEngine.GUI.skin = HighLogic.Skin;
                windowpos = GUILayout.Window(GetInstanceID(), windowpos, WindowGUI, "Kerbal Simpit", GUILayout.Width(200));
            }
        }
    }
}
