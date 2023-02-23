using DaftAppleGames.SeatruckRecall_BZ.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DaftAppleGames.SeatruckRecall_BZ.DockRecaller
{
    /// <summary>
    /// MonoBehaviour class implementing the UI elements of the
    /// Seatruck Recall component
    /// </summary>
    internal class SeaTruckDockRecallerUi : MonoBehaviour
    {
        private const string RecallInactiveText = "RECALL INACTIVE";
        private const string RecallButtonText = "RECALL SEATRUCK";
        private const string NoSeaTrucksText = "NONE IN RANGE!";
        private const string RecallingText = "RECALL IN PROGRESS..";
        private const string StuckText = "RECALL STUCK...";
        private const string AbortedText = "RECALL ABORTED!";
        private const string DistanceText = "DISTANCE: {}";

        // UI properties
        private MoonpoolExpansionTerminal _expansionTerminal;
        private GameObject _editScreenGo;
        private GameObject _activeScreenGo;
        private GameObject _inactiveScreenGo;
        private GameObject _dockingStatusTextGo;
        private TextMeshProUGUI _dockingStatusText;
        private GameObject _recallButtonGo;

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
            _seatruckRecaller.OnDockRecallStatusChangedEvent.AddListener(UpdateStatusHandler);
        }

        /// <summary>
        /// Create the Recall UI
        /// </summary>
        private void CreateUi()
        {
            SeaTruckDockRecallPlugin.Log.LogDebug("Creating UI...");

            // Get the InfoPanel so we can parent things nice
            TextMeshProUGUI infoPanel = _expansionTerminal.infoPanel;

            // Create the status text component
            SeaTruckDockRecallPlugin.Log.LogDebug("Creating docking status text...");
            GameObject inactiveTextGo = GlobalUtils.GetNamedGameObject(_inactiveScreenGo, "Text");
            _dockingStatusTextGo = Instantiate(inactiveTextGo);
            _dockingStatusTextGo.transform.parent = inactiveTextGo.transform.parent;
            _dockingStatusTextGo.name = "RecallStatusText";
            _dockingStatusTextGo.transform.localPosition = new Vector3(0, -150, 0);
            _dockingStatusTextGo.transform.rotation = new Quaternion(0, 0, 0, 0);
            _dockingStatusTextGo.transform.localScale = new Vector3(1, 1, 1);
            _dockingStatusText = _dockingStatusTextGo.GetComponent<TextMeshProUGUI>();
            _dockingStatusText.text = $"{RecallInactiveText}";
            _dockingStatusText.fontSizeMax = 60;
            SeaTruckDockRecallPlugin.Log.LogDebug("Docking status text created.");

            // Create the Button Game Object and Button component
            SeaTruckDockRecallPlugin.Log.LogDebug("Creating button...");
            GameObject existingButton = GlobalUtils.GetNamedGameObject(_activeScreenGo, "Button");
            _recallButtonGo = Instantiate(existingButton);
            _recallButtonGo.name = "RecallButton";
            _recallButtonGo.transform.SetParent(_inactiveScreenGo.transform);
            _recallButtonGo.transform.localPosition = new Vector3(0, 250, 0);
            _recallButtonGo.transform.localScale = new Vector3(3, 3, 3);
            _recallButtonGo.transform.rotation = new Quaternion(0, 0, 0, 0);
            TextMeshProUGUI buttonLabel = _recallButtonGo.GetComponentInChildren<TextMeshProUGUI>();
            buttonLabel.text = $"{RecallButtonText}";
            buttonLabel.fontSizeMax = 20;
            SeaTruckDockRecallPlugin.Log.LogDebug("Button created.");

            SeaTruckDockRecallPlugin.Log.LogDebug("Setting up button handlers...");
            _recallButtonGo.GetComponent<Button>().onClick.AddListener(RecallClosestSeatruckUiHandler);
            SeaTruckDockRecallPlugin.Log.LogDebug("Button handler setup complete!");
        }

        /// <summary>
        /// Enable the Recall button
        /// </summary>
        public void EnableButton()
        {
            _recallButtonGo.GetComponent<Button>().interactable = true;
        }

        /// <summary>
        /// Disable the Recall button
        /// </summary>
        public void DisableButton()
        {
            _recallButtonGo.GetComponent<Button>().interactable = false;
        }

        /// <summary>
        /// Handle the recall button click event
        /// </summary>
        private void RecallClosestSeatruckUiHandler()
        {
            SeaTruckDockRecallPlugin.Log.LogDebug("Recall button clicked!");
            if (!_seatruckRecaller.RecallClosestSeatruck())
            {
                _dockingStatusText.text = $"{NoSeaTrucksText}";
            }
        }

        /// <summary>
        /// Handle the Recall status updates in the GUI
        /// </summary>
        /// <param name="dockRecallStatus"></param>
        private void UpdateStatusHandler(DockRecallStatus dockRecallStatus)
        {
            SeaTruckDockRecallPlugin.Log.LogDebug($"DockRecallStatus: {dockRecallStatus}");
            switch (dockRecallStatus)
            {
                case DockRecallStatus.RecallInProgress:
                    _dockingStatusText.text = $"{RecallingText}";
                    DisableButton();
                    break;
                case DockRecallStatus.Docked:
                    _dockingStatusText.text = $"{RecallInactiveText}";
                    DisableButton();
                    break;
                case DockRecallStatus.DockClear:
                    _dockingStatusText.text = $"{RecallInactiveText}";
                    EnableButton();
                    break;
            }
        }
    }
}
