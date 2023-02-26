using System.Collections.Generic;
using DaftAppleGames.Common.Ui;
using DaftAppleGames.Common.Utils;
using DaftAppleGames.SeatruckRecall_BZ.AutoPilot;
using DaftAppleGames.SeatruckRecall_BZ.Navigation;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DaftAppleGames.SeatruckRecall_BZ.DockRecaller.Ui
{
    /// <summary>
    /// MonoBehaviour class implementing the UI elements of the
    /// Seatruck Recall component
    /// </summary>
    internal class SeaTruckDockRecallerUi : MonoBehaviour
    {
        // UI labels
        private const string RecallButtonDisplayText = "RECALL SEATRUCK";

        // Dock state text
        private const string RecallDisplayText = "RECALL: ";
        private Dictionary<DockRecallState, string> _dockRecallDisplayStateTextDict = new Dictionary<DockRecallState, string>()
        {
            { DockRecallState.None, " INITIALISING..." },
            { DockRecallState.Ready, "READY" },
            { DockRecallState.Aborted, "ABORTED" },
            { DockRecallState.Recalling , "IN PROGRESS..." },
            { DockRecallState.Docked,"READY" },
            { DockRecallState.PirateDetected, "PIRATE DETECTED!" }
        };

        // Autopilot state text
        private const string AutoPilotDisplayText = "AUTOPILOT: ";
        private Dictionary<AutoPilotState, string> _autoPilotStateDisplayTextDict = new Dictionary<AutoPilotState, string>()
        {
            { AutoPilotState.None, "NOT CONNECTED" },
            { AutoPilotState.Ready, "READY" },
            { AutoPilotState.Moving, "MOVING" },
            { AutoPilotState.Arrived , "READY" },
            { AutoPilotState.RouteBlocked, "ROUTE BLOCKED!" },
            { AutoPilotState.WaypointBlocked, "WAYPOINT BLOCKED!" },
            { AutoPilotState.Paused, "PAUSED" },
            { AutoPilotState.Stopped , "STOPPED" }
        };

        // Waypoint state text
        private const string WayPointDisplayText = "WAYPOINT: ";

        // UI properties
        private MoonpoolExpansionTerminal _expansionTerminal;
        private GameObject _editScreenGo;
        private GameObject _activeScreenGo;
        private GameObject _inactiveScreenGo;

        // New text controls for state updates
        private GameObject _dockingStatusTextGo;
        private TextMeshProUGUI _dockingStatusText;

        private GameObject _autoPilotStatusTextGo;
        private TextMeshProUGUI _autoPilotStatusText;

        private GameObject _waypointTextGo;
        private TextMeshProUGUI _waypointText;

        private GameObject _recallButtonGo;
        private Button _recallButton;

        // SeatruckRecaller component
        private SeaTruckDockRecaller _seatruckRecaller;

        /// <summary>
        /// Initialise the component
        /// </summary>
        internal void Start()
        {
            _seatruckRecaller = GetComponentInParent<SeaTruckDockRecaller>();
            if (!_seatruckRecaller)
            {
                SeaTruckDockRecallPlugin.Log.LogDebug("Couldn't find SeaTruckRecaller component?!");
            }

            _expansionTerminal = GetComponent<MoonpoolExpansionTerminal>();
            if (!_expansionTerminal)
            {
                SeaTruckDockRecallPlugin.Log.LogDebug("Couldn't find ExpansionTerminal component?!");
            }

            _editScreenGo = GlobalUtils.GetNamedGameObject(gameObject, "EditScreen");
            _activeScreenGo = GlobalUtils.GetNamedGameObject(_editScreenGo, "Active");
            _inactiveScreenGo = GlobalUtils.GetNamedGameObject(_editScreenGo, "Inactive");

            CreateUi();

            // Subscribe to recaller status update
            _seatruckRecaller.OnRecallStateChanged.AddListener(UpdateStatusHandler);
        }

        /// <summary>
        /// Create the Recall UI
        /// </summary>
        private void CreateUi()
        {
            SeaTruckDockRecallPlugin.Log.LogDebug("Creating UI...");

            // Get the InfoPanel so we can parent things nice
            TextMeshProUGUI infoPanel = _expansionTerminal.infoPanel;

            // Create the status text components
            GameObject inactiveTextGo = GlobalUtils.GetNamedGameObject(_inactiveScreenGo, "Text");

            UiUtils.CloneText(inactiveTextGo, "DockingStatusText", "", -150, 50.0f, out _dockingStatusTextGo,
                out _dockingStatusText);
            UiUtils.CloneText(inactiveTextGo, "AutoPilotStatusText", "", -200, 40.0f, out _autoPilotStatusTextGo,
                out _autoPilotStatusText);
            UiUtils.CloneText(inactiveTextGo, "WaypointStatusText", "", -250, 40.0f, out _waypointTextGo,
                out _waypointText);

            // Create the Recall button components
            GameObject existingButton = GlobalUtils.GetNamedGameObject(_activeScreenGo, "Button");

            UiUtils.CloneButton(existingButton, _inactiveScreenGo.transform, "RecallButton", RecallButtonDisplayText,
                250.0f, 20.0f, 3, out _recallButtonGo, out _recallButton );

            SeaTruckDockRecallPlugin.Log.LogDebug("Setting up button handlers...");
            _recallButton.onClick.AddListener(RecallButtonHandler);
            SeaTruckDockRecallPlugin.Log.LogDebug("Button handler setup complete!");
        }


        /// <summary>
        /// Enable the Recall UI
        /// </summary>
        public void EnableUi()
        {
            _recallButton.interactable = true;
        }

        /// <summary>
        /// Disable the Recall UI
        /// </summary>
        public void DisableUi()
        {
            _recallButton.interactable = false;
        }

        /// <summary>
        /// Handle the recall button click event
        /// </summary>
        private void RecallButtonHandler()
        {
            SeaTruckDockRecallPlugin.Log.LogDebug("Recall button clicked!");
            _seatruckRecaller.RecallClosestSeatruck();
        }

        /// <summary>
        /// Handle the Recall status updates in the GUI
        /// </summary>
        /// <param name="dockRecallState"></param>
        private void UpdateStatusHandler(DockRecallState dockRecallState, AutoPilotState autoPilotState, Waypoint waypoint)
        {
            // Update the UI
            if (waypoint != null)
            {
                SeaTruckDockRecallPlugin.Log.LogDebug($"Updating UI...: {dockRecallState.ToString()}, {autoPilotState.ToString()}, {waypoint.Name}");
            }
            else
            {
                SeaTruckDockRecallPlugin.Log.LogDebug($"Updating UI...: {dockRecallState.ToString()}, {autoPilotState.ToString()}");
            }

            _dockingStatusText.text = $"{RecallDisplayText}{_dockRecallDisplayStateTextDict[dockRecallState]}";
            _autoPilotStatusText.text = $"{AutoPilotDisplayText}{_autoPilotStateDisplayTextDict[autoPilotState]}";
            if (waypoint != null)
            {
                _waypointText.text = $"{WayPointDisplayText}{waypoint.Name}";
            }

            // Enable or disable UI components
            switch (dockRecallState)
            {
                case DockRecallState.Ready:
                case DockRecallState.None:
                case DockRecallState.NoneInRange:
                    EnableUi();
                    break;
                default:
                    DisableUi();
                    break;
            }
        }
    }
}
