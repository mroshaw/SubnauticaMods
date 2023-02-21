using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DaftAppleGames.SeatruckRecall_BZ.MonoBehaviours
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

        // UI properties
        private MoonpoolExpansionTerminal _expansionTerminal;
        private GameObject _editScreenGo;
        private GameObject _activeScreenGo;
        private GameObject _inactiveScreenGo;
        private GameObject _inactiveTextGo;
        private TextMeshProUGUI _inactiveText;
        private string _origStatusText;

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

            _editScreenGo = GetNamedGameObject(gameObject, "EditScreen");
            _activeScreenGo = GetNamedGameObject(_editScreenGo, "Active");
            _inactiveScreenGo = GetNamedGameObject(_editScreenGo, "Inactive");
            _inactiveTextGo = GetNamedGameObject(_inactiveScreenGo, "Text");
            CreateUi();

            // Subscribe to recaller status update
            _seatruckRecaller.OnRecallUpdatedEvent.AddListener(UpdateStatusHandler);
  }

        /// <summary>
        /// Find the InfoPanel Game Object
        /// </summary>
        /// <returns></returns>
        private GameObject GetNamedGameObject(GameObject parent, string childName)
        {
            foreach (Transform child in parent.GetComponentsInChildren<Transform>(true))
            {
                if (child.gameObject.name == childName)
                {
                    SeaTruckDockRecallPlugin.Log.LogDebug($"Found {childName} GameObject...");
                    return child.gameObject;
                }
            }
            return null;
        }

        /// <summary>
        /// Create the UI
        /// </summary>
        private void CreateUi()
        {
            SeaTruckDockRecallPlugin.Log.LogDebug("Creating UI...");

            // Get the InfoPanel so we can parent things nice
            TextMeshProUGUI infoPanel = _expansionTerminal.infoPanel;

            // Update inactive text
            SeaTruckDockRecallPlugin.Log.LogDebug("Updating inactive text...");
            _inactiveText = _inactiveTextGo.GetComponent<TextMeshProUGUI>();
            _origStatusText = _inactiveText.text;
            _inactiveText.text = $"{_origStatusText}\n{RecallInactiveText}";
            SeaTruckDockRecallPlugin.Log.LogDebug("Inactive text updated.");

            // Create the Button Game Object and Button component
            SeaTruckDockRecallPlugin.Log.LogDebug("Creating button...");
            GameObject existingButton = GetNamedGameObject(_activeScreenGo, "Button");
            GameObject newButton = Instantiate(existingButton);
            newButton.name = "RecallButton";
            newButton.transform.SetParent(_inactiveScreenGo.transform);
            newButton.transform.position = _inactiveText.transform.position;
            newButton.transform.localPosition = new Vector3(0, 250, 0);
            newButton.transform.localScale = new Vector3(3, 3, 3);
            newButton.transform.rotation = new Quaternion(0, 0, 0, 0);
            TextMeshProUGUI buttonLabel = newButton.GetComponentInChildren<TextMeshProUGUI>();
            buttonLabel.text = $"{RecallButtonText}";
            buttonLabel.fontSizeMax = 20;
            SeaTruckDockRecallPlugin.Log.LogDebug("Button created.");

            SeaTruckDockRecallPlugin.Log.LogDebug("Setting up button handlers...");
            newButton.GetComponent<Button>().onClick.AddListener(RecallClosestSeatruckUiHandler);
            SeaTruckDockRecallPlugin.Log.LogDebug("Button handler setup complete!");
        }

        public void EnableButton()
        {

        }

        public void DisableButton()
        {

        }

        /// <summary>
        /// Handle the recall button click event
        /// </summary>
        private void RecallClosestSeatruckUiHandler()
        {
            SeaTruckDockRecallPlugin.Log.LogDebug("Recall button clicked!");
            if (!_seatruckRecaller.RecallClosestSeatruck())
            {
                _inactiveText.text = $"{_origStatusText}\n{NoSeaTrucksText}";
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
                    _inactiveText.text = $"{_origStatusText}\n{RecallingText}";
                    break;
            }
        }
    }
}
