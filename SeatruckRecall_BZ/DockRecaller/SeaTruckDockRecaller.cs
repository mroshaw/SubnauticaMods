﻿using System.Collections.Generic;
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
        Stuck,
        Aborted,
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

        // Internal fields
        private List<Waypoint> _dockingWaypoints;

        // Event publishing latest recall state and distance
        internal DockRecallStateChangedEvent OnDockingStateChanged = new DockRecallStateChangedEvent();
        internal DockingWaypointChangedEvent OnDockingWaypointChanged = new DockingWaypointChangedEvent();
        internal AutoPilotStateChangedEvent OnAutoPilotStateChanged = new AutoPilotStateChangedEvent();
        /// <summary>
        /// Initialise the component
        /// </summary>
        internal void Start()
        {
            // Init useful local components
            _dockingManager = GetComponent<MoonpoolExpansionManager>();

            // Set the initial dock status
            SetCurrentDockedStatus();

            // Set up the docking waypoints
            CreateWaypoints();
        }

        protected internal void Docked()
        {
            SetDockState(DockRecallState.Docked);
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
            Plugin.Log.LogDebug($"DockRecaller.AutoPilotStateChangedHandler: {autoPilotState}.");

            // Handle case when AutoPilot is docked
            switch (autoPilotState)
            {
                case AutoPilotState.Arrived:
                    UnsetDockingAutoPilot();
                    SetDockState(DockRecallState.Docked);
                    break;
                case AutoPilotState.Moving:
                case AutoPilotState.WaypointReached:
                    SetDockState(DockRecallState.Recalling);
                    break;
                case AutoPilotState.RouteBlocked:
                case AutoPilotState.WaypointBlocked:
                case AutoPilotState.Aborted:
                    _currentRecallState = DockRecallState.Ready;
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
            _currentRecallState = newRecallState;
            OnDockingStateChanged?.Invoke(newRecallState);
        }

        /// <summary>
        /// Public method to cancel in-progress Recall
        /// </summary>
        internal void AbortRecall()
        {
            Plugin.Log.LogDebug("Aborting Recall...");
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

            Plugin.Log.LogDebug("Finding closest Seatruck...");
            _currentRecallAutoPilot = AutoPilots.GetClosestAutoPilot(transform.position, MaxRange);
            if (_currentRecallAutoPilot == null)
            {
                // Couldn't find a closest Seatruck
                Plugin.Log.LogDebug("No Seatrucks found!");
                _currentRecallState = DockRecallState.NoneInRange;
                return;
            }

            // Recall the SeaTruck
            SetDockingAutoPilot(_currentRecallAutoPilot);
            _currentRecallAutoPilot.BeginNavigation(_dockingWaypoints);
        }
    }
}