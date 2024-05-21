using System.Collections.Generic;
using DaftAppleGames.Common.Utils;
using DaftAppleGames.SeatruckRecall_BZ.AutoPilot;
using DaftAppleGames.SeatruckRecall_BZ.Navigation;
using DaftAppleGames.SeatruckRecall_BZ.Utils;
using UnityEngine;
using UnityEngine.Events;
using Plugin = DaftAppleGames.SeatruckRecall_BZ.SeaTruckDockRecallPlugin;

namespace DaftAppleGames.SeatruckRecall_BZ.DockRecaller
{
    // Recaller Status
    internal enum DockRecallState
    {
        None,
        Ready,
        NoneInRange,
        Recalling,
        Aborted,
        Docked,
        PirateDetected
    }

    // Unity Event to publish DockRecallStatus changes
    internal class DockRecallStatusChangedEvent : UnityEvent<DockRecallState, AutoPilotState, Waypoint>
    {
    }

    /// <summary>
    /// MonoBehaviour class to attach to a SeatruckDock
    /// that implements the recall behaviour
    /// </summary>
    internal class SeaTruckDockRecaller : MonoBehaviour
    {
        // Waypoint names
        private const string MoveToBaseText = "MOVING TO BASE";
        private const string AlignToDockText = "ALIGNING TO DOCK";
        private const string MovingToDockText = "MOVING TO DOCK";

        // Useful internal components
        private MoonpoolExpansionManager _dockingManager;

        // Public properties
        public float MaxRange { get; set; } = 500.0f;

        // Internal tracking and audit
        private DockRecallState _currentRecallState = DockRecallState.None;
        private DockRecallState _previousRecallState;
        private BaseAutoPilot _currentRecallAutoPilot;
        private AutoPilotState _currentAutoPilotState;
        private Waypoint _currentWaypoint;

        // Internal fields
        private List<Waypoint> _dockingWaypoints;

        // Event publishing latest recall state and distance
        internal DockRecallStatusChangedEvent OnRecallStateChanged = new DockRecallStatusChangedEvent();

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
            _dockingWaypoints.Add(new Waypoint(aboveDockingTubeWaypoint.transform,
                WaypointAction.RotateBeforeMove,
                MoveToBaseText));

            Plugin.Log.LogDebug($"Dock tube above end position: {aboveDockingTubeWaypoint.transform.position}");

            // Waypoint at the end of the docking tube.
            GameObject endOfDockTubeWaypoint = new GameObject("End of Tube Waypoint")
            {
                transform =
                {
                    position = gameObject.transform.position + (-gameObject.transform.right * 50.0f)
                }
            };
            _dockingWaypoints.Add(new Waypoint(endOfDockTubeWaypoint.transform,
                WaypointAction.RotateBeforeMove,
                AlignToDockText));

            Plugin.Log.LogDebug($"Dock tube end position: {endOfDockTubeWaypoint.transform.position}");

            // Waypoint into the docking tube itself
            GameObject dockingWaypoint = new GameObject("Docking Waypoint")
            {
                transform =
                {
                    position = gameObject.transform.position + (-gameObject.transform.right * 15.0f)
                }
            };
            _dockingWaypoints.Add(new Waypoint(dockingWaypoint.transform,
                WaypointAction.RotateBeforeMove,
                MovingToDockText));

            Plugin.Log.LogDebug($"Dock final position: {dockingWaypoint.transform.position}");
        }

        /// <summary>
        /// Handle updating current state, if recall is underway
        /// </summary>
        internal void Update()
        {
            CheckState();
        }

        /// <summary>
        /// Checks for changes in the recall status
        /// </summary>
        /// <returns></returns>
        private void CheckState()
        {
            if (_previousRecallState != _currentRecallState)
            {
                _previousRecallState = _currentRecallState;
                StateOrWaypointChanged();
            }
        }
    
        /// <summary>
        /// Called whenever state changes
        /// </summary>
        private void StateOrWaypointChanged()
        {
            // Update any listeners
            if (OnRecallStateChanged != null)
            {
                OnRecallStateChanged.Invoke(_currentRecallState, _currentAutoPilotState, _currentWaypoint);
            }

            switch (_currentRecallState)
            {
                case DockRecallState.Aborted:
                    _currentRecallAutoPilot.AbortNavigation();
                    break;
            }
        }

        /// <summary>
        /// Sets the appropriate docked status
        /// </summary>
        private void SetDockedStatus()
        {
            if (IsDockOccupied())
            {
                _currentRecallState = DockRecallState.Docked;
            }
            else
            {
                _currentRecallState = DockRecallState.Ready;
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
            _currentRecallState = DockRecallState.Docked;
        }

        /// <summary>
        /// Set the current status to DockClear
        /// </summary>
        internal void SetUndocked()
        {
            _currentRecallState = DockRecallState.Ready;
        }

        /// <summary>
        /// Handle Waypoint change
        /// </summary>
        /// <param name="waypoint"></param>
        private void AutoPilotStateChangedHandler(AutoPilotState autoPilotState, Waypoint waypoint)
        {
            Plugin.Log.LogDebug($"DockRecaller.AutoPilotStateChangedHandler: {autoPilotState}.");
            _currentAutoPilotState = autoPilotState;
            _currentWaypoint = waypoint;

            // Handle case when AutoPilot is docked
            switch (_currentAutoPilotState)
            {
                case AutoPilotState.Arrived:
                    _currentRecallState = DockRecallState.Docked;
                    _currentRecallAutoPilot.OnAutopilotStatusChanged.RemoveAllListeners();
                    break;
                case AutoPilotState.Moving:
                case AutoPilotState.WaypointReached:
                    _currentRecallState = DockRecallState.Recalling;
                    break;
                case AutoPilotState.RouteBlocked:
                case AutoPilotState.WaypointBlocked:
                case AutoPilotState.Aborted:
                    _currentRecallState = DockRecallState.Ready;
                    break;
            }
            StateOrWaypointChanged();
        }

        /// <summary>
        /// Public method to cancel in-progress Recall
        /// </summary>
        internal void AbortRecall()
        {
            Plugin.Log.LogDebug("Aborting Recall...");
            _currentRecallState = DockRecallState.Aborted;
        }

        /// <summary>
        /// Public method to recall the closest Seatruck
        /// </summary>
        /// <returns></returns>
        internal void RecallClosestSeatruck()
        {
            // Check for pirate
            if (GlobalUtils.IsPirate())
            {
                _currentRecallState = DockRecallState.PirateDetected;
                return;
            }

            Plugin.Log.LogDebug("Finding closest Seatruck...");
            _currentRecallAutoPilot = GetClosestSeaTruck();
            if (_currentRecallAutoPilot == null)
            {
                // Couldn't find a closest Seatruck
                Plugin.Log.LogDebug("No Seatrucks found!");
                _currentRecallState = DockRecallState.NoneInRange;
                return;
            }

            // Recall the SeaTruck
            _currentRecallAutoPilot.OnAutopilotStatusChanged.AddListener(AutoPilotStateChangedHandler);
            _currentRecallAutoPilot.BeginNavigation(_dockingWaypoints);
        }

        /// <summary>
        /// Return the registered Seatruck that's closest to the Dock
        /// </summary>
        /// <returns></returns>
        private BaseAutoPilot GetClosestSeaTruck()
        {
            Transform currentTransform = transform;
            BaseAutoPilot closestSeaTruck = null;
            float closestDistance = 0.0f;

            // Don't need to do anything if there are no Seatrucks
            if (ModUtils.AllSeaTruckAutoPilots.Count == 0)
            {
                Plugin.Log.LogInfo("No Seatrucks registered.");
                _currentRecallState = DockRecallState.NoneInRange;

                return null;
            }

            Plugin.Log.LogInfo($"Looking for SeaTrucks. Max range is: {MaxRange}");

            // Loop through each seatruck, find out which is closest
            foreach (BaseAutoPilot seatruck in ModUtils.AllSeaTruckAutoPilots)
            {
                // Check if already docked
                SeaTruckSegment segment = seatruck.GetComponent<SeaTruckSegment>();
                if (segment.isDocked || seatruck.AutoPilotInProgress)
                {
                    Plugin.Log.LogInfo($"Seatruck {seatruck.gameObject.name} is already docking or docked. Skipping...");
                    continue;
                }

                Plugin.Log.LogInfo($"Checking distance to {seatruck.gameObject.name}...");
                float currDistance = Vector3.Distance(currentTransform.position, seatruck.gameObject.transform.position);
                {
                    Plugin.Log.LogInfo($"Distance is: {currDistance}, closest so far is: {closestDistance}");
                    if ((closestDistance == 0 || currDistance < closestDistance) && currDistance <= MaxRange)
                    {
                        Plugin.Log.LogInfo("New closest Seatruck found!");
                        closestDistance = currDistance;
                        closestSeaTruck = seatruck;;
                    }
                }
            }
            
            // Check to see if we've found anything in range
            if (closestSeaTruck == null)
            {
                Plugin.Log.LogInfo($"No SeaTrucks found within range!");
            }
            else
            {
                Plugin.Log.LogInfo($"Closest Seatruck found: {closestSeaTruck.gameObject.name} at {closestDistance}");
            }
            
            return closestSeaTruck;
        }
    }
}
