using System.Collections;
using System.Collections.Generic;
using DaftAppleGames.SeatruckRecall_BZ.AutoPilot;
using DaftAppleGames.SeatruckRecall_BZ.Navigation;
using DaftAppleGames.SeatruckRecall_BZ.Utils;
using UnityEngine;
using UnityEngine.Events;
using static DaftAppleGames.SeatruckRecall_BZ.SeaTruckDockRecallPlugin;

namespace DaftAppleGames.SeatruckRecall_BZ.DockRecaller
{
    // Recaller Status
    internal enum DockRecallState
    {
        None,
        Ready,
        NoneInRange,
        Recalling,
        Stuck,
        Aborted,
        Parking,
        Docked,
        PirateDetected
    }

    // Unity Event to publish DockRecallStatus changes
    internal class DockRecallStateChangedEvent : UnityEvent<DockRecallState>
    {
    }

    internal class AutoPilotStateChangedEvent : UnityEvent<AutoPilotState>
    {
    }

    internal class DockingWaypointChangedEvent : UnityEvent<Waypoint>
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
        private SeaTruckAutoPilot _currentRecallAutoPilot;

        // Transform within the dock, that the recall will pull the SeaTruck into it's final docking place
        // If not docked within the timeout, abandon
        private Vector3 _parkingDockConnection;
        private const float ParkingTimeout = 5.0f;
        private const float ParkingMoveSpeed = 1.0f;
        private const float ParkingRotateSpeed = 1.0f;

        // Internal fields
        private List<Waypoint> _dockingWaypoints;

        // Event publishing latest recall state and distance
        internal DockRecallStateChangedEvent OnDockingStateChanged = new DockRecallStateChangedEvent();
        internal DockingWaypointChangedEvent OnDockingWaypointChanged = new DockingWaypointChangedEvent();
        internal AutoPilotStateChangedEvent OnAutoPilotStateChanged = new AutoPilotStateChangedEvent();

        internal UnityEvent OnDocked = new UnityEvent();

        /// <summary>
        /// Initialise the component
        /// </summary>
        internal void Start()
        {
            // Init useful local components
            _dockingManager = GetComponent<MoonpoolExpansionManager>();

            // Set the initial dock status
            SetCurrentDockedStatus();

            // Set the parking position
            _parkingDockConnection = gameObject.transform.position + (-gameObject.transform.right * 2.0f);

            // Set up the docking waypoints
            CreateWaypoints();
        }

        protected internal void Docked()
        {
            SetDockState(DockRecallState.Docked);
            UnsetDockingAutoPilot();
            OnDocked?.Invoke();
        }

        protected internal void Undocked()
        {
            SetDockState(DockRecallState.Ready);
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
            _dockingWaypoints.Add(new Waypoint(aboveDockingTubeWaypoint.transform.position,
                Quaternion.identity,
                true,
                MoveToBaseText));

            Log.LogDebug($"Dock tube above end position: {aboveDockingTubeWaypoint.transform.position}");

            // Waypoint at the end of the docking tube.
            GameObject endOfDockTubeWaypoint = new GameObject("End of Tube Waypoint")
            {
                transform =
                {
                    position = gameObject.transform.position + (-gameObject.transform.right * 50.0f)
                }
            };
            _dockingWaypoints.Add(new Waypoint(endOfDockTubeWaypoint.transform.position,
                Quaternion.identity,
                true,
                AlignToDockText));

            Log.LogDebug($"Dock tube end position: {endOfDockTubeWaypoint.transform.position}");

            // Waypoint into the docking tube itself
            GameObject dockingWaypoint = new GameObject("Docking Waypoint")
            {
                transform =
                {
                    position = gameObject.transform.position + (-gameObject.transform.right * 15.0f)
                }
            };
            _dockingWaypoints.Add(new Waypoint(dockingWaypoint.transform.position,
                Quaternion.identity,
                true,
                MovingToDockText));

            Log.LogDebug($"Dock final position: {dockingWaypoint.transform.position}");
        }

        private void SetDockingAutoPilot(SeaTruckAutoPilot autoPilot)
        {
            _currentRecallAutoPilot = autoPilot;
            autoPilot.OnAutopilotStateChanged.AddListener(AutoPilotStateChangedHandler);
            autoPilot.OnAutopilotWaypointChanged.AddListener(AutopilotWaypointChangedHandler);
        }

        private void UnsetDockingAutoPilot()
        {
            _currentRecallAutoPilot.OnAutopilotStateChanged.RemoveListener(AutoPilotStateChangedHandler);
            _currentRecallAutoPilot.OnAutopilotWaypointChanged.RemoveListener(AutopilotWaypointChangedHandler);
            _currentRecallAutoPilot = null;
        }

        private void AutopilotWaypointChangedHandler(Waypoint newWaypoint)
        {
            OnDockingWaypointChanged?.Invoke(newWaypoint);
        }

        /// <summary>
        /// Sets the appropriate docked status
        /// </summary>
        private void SetCurrentDockedStatus()
        {
            SetDockState(IsDockOccupied() ? DockRecallState.Docked : DockRecallState.Ready);
        }

        /// <summary>
        /// Returns true if Seatruck is already docked here
        /// otherwise false
        /// </summary>
        private bool IsDockOccupied()
        {
            return _dockingManager.IsOccupied();
        }

        private void AutoPilotWaypointChangedHandler(Waypoint waypoint)
        {
            OnDockingWaypointChanged.Invoke(waypoint);
        }

        /// <summary>
        /// Handle Waypoint change
        /// </summary>
        private void AutoPilotStateChangedHandler(AutoPilotState autoPilotState)
        {
            Log.LogDebug($"DockRecaller.AutoPilotStateChangedHandler: {autoPilotState}.");

            // Autopilot state changes
            switch (autoPilotState)
            {
                // AutoPilot has arrived. Docking isn't guaranteed, so we'll check that and engage
                // the tractor beam if required
                case AutoPilotState.Arrived:
                    SetDockState(DockRecallState.Parking);
                    ParkSeaTruck();
                    break;
                case AutoPilotState.Moving:
                case AutoPilotState.WaypointReached:
                    SetDockState(DockRecallState.Recalling);
                    break;
                case AutoPilotState.RouteBlocked:
                case AutoPilotState.WaypointBlocked:
                case AutoPilotState.Aborted:
                    SetDockState(DockRecallState.Ready);
                    break;
            }

            OnAutoPilotStateChanged.Invoke(autoPilotState);
        }

        private void SetDockState(DockRecallState newRecallState)
        {
            if (newRecallState == _currentRecallState)
            {
                return;
            }

            Log.LogDebug($"SeaTruckRecaller.SetDockState: state changed from {_currentRecallState} to {newRecallState}.");
            _currentRecallState = newRecallState;
            OnDockingStateChanged?.Invoke(newRecallState);
        }

        /// <summary>
        /// Public method to cancel in-progress Recall
        /// </summary>
        internal void AbortRecall()
        {
            Log.LogDebug("Aborting Recall...");
            SetDockState(DockRecallState.Aborted);
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

            Log.LogDebug("Finding closest Seatruck...");
            _currentRecallAutoPilot = AllAutoPilots.GetClosestAutoPilot(transform.position, MaxRange);
            if (_currentRecallAutoPilot == null)
            {
                // Couldn't find a closest Seatruck
                Log.LogDebug("No Seatrucks found!");
                _currentRecallState = DockRecallState.NoneInRange;
                return;
            }

            // Recall the SeaTruck
            SetDockingAutoPilot(_currentRecallAutoPilot);
            _currentRecallAutoPilot.BeginNavigation(_dockingWaypoints);
        }

        /// <summary>
        /// Pulls the SeaTruck towards the dock, forcing it to engage and dock
        /// </summary>
        private void ParkSeaTruck()
        {
            StartCoroutine(ParkSeaTruckAsync());
        }

        private IEnumerator ParkSeaTruckAsync()
        {
            Log.LogDebug("Parking SeaTruck...");
            float dockTime = 0.0f;

            if (_currentRecallAutoPilot == null)
            {
                Log.LogDebug("Parking cancelled - SeaTruck not set");
                yield break;
            }

            Vector3 dirToTarget = _parkingDockConnection - _currentRecallAutoPilot.transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(dirToTarget);

            while (_currentRecallState != DockRecallState.Docked)
            {
                // Rotate
                _currentRecallAutoPilot.transform.rotation = Quaternion.Slerp(_currentRecallAutoPilot.transform.rotation, targetRotation, Time.deltaTime * ParkingRotateSpeed);
                dockTime += Time.deltaTime;

                // Move
                _currentRecallAutoPilot.transform.position = Vector3.Lerp(_currentRecallAutoPilot.transform.position, _parkingDockConnection, Time.deltaTime * ParkingMoveSpeed);

                if (dockTime > ParkingTimeout)
                {
                    Log.LogDebug("Parking timed out!");
                    SetDockState(DockRecallState.Stuck);
                    yield break;
                }

                yield return null;
            }

            Log.LogDebug("Docked state set: Parking Complete!");
        }
    }
}