using UnityEngine;
using UnityEngine.UI;

namespace DaftAppleGames.SeatruckRecall_BZ.MonoBehaviours
{
    /// <summary>
    /// MonoBehaviour class implementing the UI elements of the
    /// Seatruck Recall component
    /// </summary>
    internal class SeaTruckRecallerUi : MonoBehaviour
    {
        // UI properies
        private Button _recallButton;
        private Canvas _uiCanvas;

        // SeatruckRecaller component
        public SeaTruckDockRecaller seatruckRecaller;

        /// <summary>
        /// Initialise the component
        /// </summary>
        public void Start()
        {
            seatruckRecaller = GetComponent<SeaTruckDockRecaller>();
            CreateUi();
            ConfigureUiEvents();
        }

        /// <summary>
        /// Find and set the Unity canvas to display the button
        /// </summary>
        private void FindParentCanvas()
        {

        }

        /// <summary>
        /// Create the UI
        /// </summary>
        public void CreateUi()
        {

        }

        /// <summary>
        /// Hide the button UI
        /// </summary>
        public void ShowUi()
        {

        }

        /// <summary>
        /// Show the button UI
        /// </summary>
        public void HideUi()
        {

        }

        /// <summary>
        /// Set up the button listener events
        /// </summary>
        public void ConfigureUiEvents()
        {
            _recallButton.onClick.AddListener(RecallClosestSeatruckUiHandler);
        }

        /// <summary>
        /// Handle the recall button click event
        /// </summary>
        private void RecallClosestSeatruckUiHandler()
        {
            seatruckRecaller.RecallClosestSeatruck();
        }
    }
}
