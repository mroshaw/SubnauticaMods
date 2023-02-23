using System.Collections.Generic;
using DaftAppleGames.SeatruckRecall_BZ.Navigation;
using UnityEngine;
using UnityEngine.Events;

namespace DaftAppleGames.SeatruckRecall_BZ.AutoPilot
{
    // AutoPilot states
    internal enum AutoPilotState
    {
        Ready,
        Moving,
        Arrived,
        Blocked
    };

    /// <summary>
    /// MonoBehavior implementing AutoPilot behaviour
    /// </summary>
    internal class BaseAutoPilot : MonoBehaviour
    {
        // Unity Event to publish AutoPilot state changes
        internal class AutopilotStatusChangedEvent : UnityEvent<AutoPilotState, string>
        {
        }

        // Public properties
        internal AutoPilotState CurrentState { get; private set; }
        internal Waypoint CurrentWaypoint { get; private set; }
        internal INavMovement NavMovement { get; set; }

        // Private fields
        private WaypointNavigation _waypointNav;

        // Unity Event publishing Status changes
        internal AutopilotStatusChangedEvent OnAutopilotStatusChangedEvent = new AutopilotStatusChangedEvent();

        /// <summary>
        /// Initialise the component
        /// </summary>
        internal virtual void Start()
        {
            // Get the WaypointNavigation component
            _waypointNav = GetComponent<WaypointNavigation>();

            // Set default state
            CurrentState = AutoPilotState.Ready;

            // Subscribe to Waypoint changed event
            _waypointNav.OnWaypointChangedEvent.AddListener(WaypointChangedHandler);

            // Set up the Nav
        }



        /// <summary>
        /// Method to call the Seatruck to the given dock
        /// </summary>
        /// <param name="seaTruckRecaller"></param>
        /// <param name="waypoints"></param>
        /// <returns></returns>
        internal bool BeginNavigation(List<Waypoint> waypoints)
        {
            // Abort, if already being recalled
            if (CurrentState != AutoPilotState.Ready)
            {
                // Already being recalled or is already docked
                SeaTruckDockRecallPlugin.Log.LogDebug("AutoPilot is not ready.");
                return false;
            }

            // Setup the Waypoint Nav Component
            _waypointNav.Waypoints = waypoints;
            
            // Start navigation
            _waypointNav.StartNavigation();

            return true;
        }

        /// <summary>
        /// Handle the state transitions
        /// </summary>
        internal void Update()
        {
            if (CurrentState == AutoPilotState.Ready)
            {
                return;
            }
            UpdateState();
        }

        /// <summary>
        /// Update AutoPilot state
        /// </summary>
        private void UpdateState()
        {
            // Handle the various SeaTruck states
            switch (_waypointNav.NavState)
            {
                case NavigationState.Moving:
                    CurrentState = AutoPilotState.Moving;
                    break;

                case NavigationState.FinalWaypointReached:
                    CurrentState = AutoPilotState.Arrived;
                    break;

                case NavigationState.Blocked:
                    CurrentState = AutoPilotState.Blocked;
                    break;

                default:
                    return;
            }
        }

        /// <summary>
        /// Handle Waypoint change
        /// </summary>
        /// <param name="waypoint"></param>
        private void WaypointChangedHandler(Waypoint waypoint)
        {
            CurrentWaypoint = waypoint;
            OnAutopilotStatusChangedEvent.Invoke(CurrentState, waypoint.Name);
        }
    }
}
