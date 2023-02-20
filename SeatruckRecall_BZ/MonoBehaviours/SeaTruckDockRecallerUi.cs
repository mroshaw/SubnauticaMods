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
        // UI properties
        private MoonpoolExpansionTerminal _expansionTerminal;
        private Button _recallButton;

        // SeatruckRecaller component
        private SeaTruckDockRecaller _seatruckRecaller;

        /// <summary>
        /// Initialise the component
        /// </summary>
        public void Start()
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
            CreateUi();
        }

        /// <summary>
        /// Create the UI
        /// </summary>
        public void CreateUi()
        {
            SeaTruckDockRecallPlugin.Log.LogDebug("Creating UI...");
            // Get the InfoPanel so we can parent things nice
            TextMeshProUGUI infoPanel = _expansionTerminal.infoPanel;

            // Create the Button Game Object and Button component
            SeaTruckDockRecallPlugin.Log.LogDebug("Creating button...");
            GameObject recallButtonGameObject = new GameObject("RecallButton");
            recallButtonGameObject.transform.SetParent(infoPanel.gameObject.transform.parent.transform);
            _recallButton = recallButtonGameObject.AddComponent<Button>();
            SeaTruckDockRecallPlugin.Log.LogDebug("Button created.");

            // Create the Label Game Object child and component
            SeaTruckDockRecallPlugin.Log.LogDebug("Creating label...");
            GameObject buttonLabelGameObject = new GameObject("RecallButtonLabel");
            buttonLabelGameObject.transform.SetParent(recallButtonGameObject.transform);
            TextMeshProUGUI buttonLabel = buttonLabelGameObject.AddComponent<TextMeshProUGUI>();
            buttonLabel.text = "Recall SeaTruck";
            SeaTruckDockRecallPlugin.Log.LogDebug("Label created.");

            SeaTruckDockRecallPlugin.Log.LogDebug("Setting up button handlers...");
            _recallButton.onClick.AddListener(RecallClosestSeatruckUiHandler);
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

        }
    }
}
