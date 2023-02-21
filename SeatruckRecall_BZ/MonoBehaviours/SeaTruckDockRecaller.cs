using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DaftAppleGames.SeatruckRecall_BZ.MonoBehaviours
{
    // Recaller Status
    internal enum DockRecallStatus
    {
        None,
        Docked,
        DockClear,
        RecallInProgress,
        RecallAborted,
        NoSeaTrucks,
        NoneInRange,
        FoundClosestSeaTruck
    }

    internal class DockRecallStatusEvent : UnityEvent<DockRecallStatus>
    {
    }

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
        private List<Waypoint> _dockingWaypoints;

        // Event triggered with latest recall state and distance
        internal DockRecallStatusEvent OnRecallUpdatedEvent = new DockRecallStatusEvent();

        /// <summary>
        /// Initialise the component
        /// </summary>
        internal void Start()
        {
            // Init useful local components
            _dockingManager = GetComponent<MoonpoolExpansionManager>();

            // Set the initial dock status
            SetDockedStatus();

            // Set up the docking waypoints
            CreateWaypoints();
        }

        /// <summary>
        /// Set up the docking waypoints for this dock
        /// </summary>
        private void CreateWaypoints()
        {
            _dockingWaypoints = new List<Waypoint>();

            // Waypoint above the entrance to the docking tube
            GameObject aboveDockingTubeWaypoint = new GameObject("Top of End of Tube Waypoint")
            {
                transform =
                {
                    position = gameObject.transform.position + (-gameObject.transform.right * 30.0f) + (gameObject.transform.up * 10.0f)
                }
            };
            _dockingWaypoints.Add(new Waypoint(aboveDockingTubeWaypoint.transform, WaypointAction.RotateBeforeMove));
            SeaTruckDockRecallPlugin.Log.LogDebug($"Dock tube above end position: {aboveDockingTubeWaypoint.transform.position}");

            // Waypoint at the end of the docking tube.
            GameObject endOfDockTubeWaypoint = new GameObject("End of Tube Waypoint")
            {
                transform =
                {
                    position = gameObject.transform.position + (-gameObject.transform.right * 30.0f)
                }
            };
            _dockingWaypoints.Add(new Waypoint(endOfDockTubeWaypoint.transform, WaypointAction.RotateBeforeMove));
            SeaTruckDockRecallPlugin.Log.LogDebug($"Dock tube end position: {endOfDockTubeWaypoint.transform.position}");

            // Waypoint into the docking tube itself
            GameObject dockingWaypoint = new GameObject("Docking Waypoint")
            {
                transform =
                {
                    position = gameObject.transform.position + (-gameObject.transform.right * 11.0f)
                }
            };
            _dockingWaypoints.Add(new Waypoint(dockingWaypoint.transform, WaypointAction.RotateBeforeMove));
            SeaTruckDockRecallPlugin.Log.LogDebug($"Dock final position: {dockingWaypoint.transform.position}");
        }

        /// <summary>
        /// Handle updating current state, if recall is underway
        /// </summary>
        internal void Update()
        {
            // DEBUG ONLY!!!
            if (SeaTruckDockRecallPlugin.RecallKeyboardShortcut.Value.IsDown())
            {
                RecallClosestSeatruck();
            }
        }

        /// <summary>
        /// Called whenever state changes
        /// </summary>
        private void RecallStatusChanged()
        {
            // Update any listeners
            if (OnRecallUpdatedEvent != null)
            {
                OnRecallUpdatedEvent.Invoke(_latestRecallStatus);
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
            RecallStatusChanged();
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
            RecallStatusChanged();

            // Recall the SeaTruck
            _currentRecallAutoPilot.RecallToDock(this, _dockingWaypoints);

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

            float maxRange = SeaTruckDockRecallPlugin.MaximumRange.Value;

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
