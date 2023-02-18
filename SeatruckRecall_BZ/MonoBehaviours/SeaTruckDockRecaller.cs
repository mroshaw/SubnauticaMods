using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DaftAppleGames.SeatruckRecall_BZ.MonoBehaviours
{
    /// <summary>
    /// MonoBehaviour class to attach to a SeatruckDock
    /// that implements the recall behaviour
    /// </summary>
    internal class SeaTruckDockRecaller : MonoBehaviour
    {
        // Useful internal components
        private MoonpoolExpansionManager _dockingManager;

        // Internal tracking and audit
        private DockRecallStatus _latestRecallStatus = DockRecallStatus.None;
        private SeaTruckAutoPilot _currentRecallAutoPilot;

        // Internal properties
        private Vector3 _endOfDocktubePosition;

        // Event triggered with latest recall state and distance
        public UnityEvent OnRecallUpdatedEvent;

        /// <summary>
        /// Initialise the component
        /// </summary>
        public void Start()
        {
            // Init useful local components
            _dockingManager = GetComponent<MoonpoolExpansionManager>();

            // Set the initial dock status
            SetDockedStatus();

            // Set the location of the end of the docking tube
            _endOfDocktubePosition = transform.position;
        }

        /// <summary>
        /// Handle updating current state, if recall is underway
        /// </summary>
        public void Update()
        {
            switch (_latestRecallStatus)
            {
                case DockRecallStatus.RecallInProgress:
                    break;

                default:
                    return;
            }
        }

        /// <summary>
        /// Called every frame when recall is in progress
        /// </summary>
        private void UpdateRecallStatus()
        {
            // Update any listeners
            if (OnRecallUpdatedEvent != null)
            {
                OnRecallUpdatedEvent.Invoke();
            }
        }

        /// <summary>
        /// Sets the appropriate docked status
        /// </summary>
        private void SetDockedStatus()
        {
            if (IsDockOccupied())
            {
                _latestRecallStatus = DockRecallStatus.Docked;
            }
            else
            {
                _latestRecallStatus = DockRecallStatus.DockClear;
            }
        }

        /// <summary>
        /// Returns true if Seatruck is already docked here
        /// otherwise false
        /// </summary>
        /// <returns></returns>
        private bool IsDockOccupied()
        {
            return _dockingManager.IsOccupied();
        }

        /// <summary>
        /// Set the current status to Docked
        /// </summary>
        internal void SetDocked()
        {
            _latestRecallStatus = DockRecallStatus.Docked;
        }

        /// <summary>
        /// Set the current status to DockClear
        /// </summary>
        internal void SetUndocked()
        {
            _latestRecallStatus = DockRecallStatus.DockClear;
        }

        /// <summary>
        /// Public method to recall the closest Seatruck
        /// </summary>
        /// <returns></returns>
        internal bool RecallClosestSeatruck()
        {
            SeaTruckDockRecallPlugin.Log.LogDebug("Finding closest Seatruck...");
            _currentRecallAutoPilot = GetClosestSeaTruck();
            if (_currentRecallAutoPilot == null)
            {
                // Couldn't find a closest Seatruck
                SeaTruckDockRecallPlugin.Log.LogDebug("No Seatrucks found!");

                return false;
            }

            // TODO
            // Set the Recall status
            SeaTruckDockRecallPlugin.Log.LogDebug("Recalling to dock...");
            _latestRecallStatus = DockRecallStatus.RecallInProgress;

            // Recall the SeaTruck
            _currentRecallAutoPilot.RecallToDock(this, _endOfDocktubePosition);

            return true;
        }

        /// <summary>
        /// Return the registered Seatruck that's closes to the Dock
        /// </summary>
        /// <returns></returns>
        private SeaTruckAutoPilot GetClosestSeaTruck()
        {
            Transform currentTransform = transform;
            SeaTruckAutoPilot closestSeaTruck = null;
            float closestDistance = 0.0f;
            List<SeaTruckAutoPilot> allSeaTrucks = SeaTruckDockRecallPlugin.GetAllAutoPilots();

            float maxRange = SeaTruckDockRecallPlugin.RecallRange.Value;

            // Don't need to do anything if there are no Seatrucks
            if (allSeaTrucks.Count == 0)
            {
                SeaTruckDockRecallPlugin.Log.LogInfo("No Seatrucks registered.");
                _latestRecallStatus = DockRecallStatus.NoSeaTrucks;
                return null;
            }

            SeaTruckDockRecallPlugin.Log.LogInfo($"Looking for SeaTrucks. Max range is: {maxRange}");

            // Loop through each seatruck, find out which is closest
            foreach (SeaTruckAutoPilot seatruck in allSeaTrucks)
            {
                // Check if already docked
                SeaTruckSegment segment = seatruck.GetComponent<SeaTruckSegment>();
                if (segment.isDocked)
                {
                    SeaTruckDockRecallPlugin.Log.LogInfo($"Seatruck {seatruck.gameObject.name} is already docked. Skipping...");
                    continue;
                }

                SeaTruckDockRecallPlugin.Log.LogInfo($"Checking distance to {seatruck.gameObject.name}...");
                float currDistance = Vector3.Distance(currentTransform.position, seatruck.gameObject.transform.position);
                {
                    SeaTruckDockRecallPlugin.Log.LogInfo($"Distance is: {currDistance}, closest so far is: {closestDistance}");
                    if ((closestDistance == 0 || currDistance < closestDistance) && currDistance <= maxRange)
                    {
                        SeaTruckDockRecallPlugin.Log.LogInfo("New closest Seatruck found!");
                        closestDistance = currDistance;
                        closestSeaTruck = seatruck;;
                    }
                }
            }
            
            // Check to see if we've found anything in range
            if (closestSeaTruck == null)
            {
                SeaTruckDockRecallPlugin.Log.LogInfo($"No SeaTrucks found within range!");
                _latestRecallStatus = DockRecallStatus.NoneInRange;
            }
            else
            {
                SeaTruckDockRecallPlugin.Log.LogInfo($"Closest Seatruck found: {closestSeaTruck.gameObject.name} at {closestDistance}");
                _latestRecallStatus = DockRecallStatus.FoundClosestSeaTruck;
            }
            
            return closestSeaTruck;
        }
    }
}
