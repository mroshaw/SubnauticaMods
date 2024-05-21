using System.Collections.Generic;
using DaftAppleGames.SeatruckRecall_BZ.Navigation;
using UnityEngine;
using UnityEngine.Events;
using Plugin = DaftAppleGames.SeatruckRecall_BZ.SeaTruckDockRecallPlugin;

namespace DaftAppleGames.SeatruckRecall_BZ.AutoPilot
{
    // AutoPilot states
    internal enum AutoPilotState
    {
        None,
        Ready,
        Moving,
        Paused,
        Stopped,
        Aborted,
        WaypointReached,
        Arrived,
        WaypointBlocked,
        RouteBlocked,
    };

    /// <summary>
    /// MonoBehavior implementing AutoPilot behaviour
    /// </summary>
    internal class BaseAutoPilot : MonoBehaviour
    {
        // Unity Event to publish AutoPilot state changes
        internal class AutopilotStatusChangedEvent : UnityEvent<AutoPilotState, Waypoint>
        {
        }

        // Public properties
        internal bool AutoPilotInProgress { get; private set; }

        // Autopilot state
        private AutoPilotState _currentAutoPilotState = AutoPilotState.None;
        private AutoPilotState _previousAutoPilotState;
        private NavState _currentNavState;

        // Private fields
        private WaypointNavigation _waypointNav;
        private Waypoint _currentWaypoint;

        // Unity Event publishing Status changes
        internal AutopilotStatusChangedEvent OnAutopilotStatusChanged = new AutopilotStatusChangedEvent();

        /// <summary>
        /// Initialise the component
        /// </summary>
        internal virtual void Start()
        {
            // Get the WaypointNavigation component
            _waypointNav = GetComponent<WaypointNavigation>();

            // Set default state
            _currentAutoPilotState = AutoPilotState.Ready;
            AutoPilotInProgress = false;

            // Subscribe to Waypoint changed event
            _waypointNav.OnNavStateChanged.AddListener(NavStateChangedHandler);
        }

        /// <summary>
        /// Method to call the Seatruck to the given dock
        /// </summary>
        /// <param name="waypoints"></param>
        /// <returns></returns>
        internal bool BeginNavigation(List<Waypoint> waypoints)
        {
            // Abort, if already being recalled
            if (_currentAutoPilotState != AutoPilotState.Ready)
            {
                // Already being recalled or is already docked
                Plugin.Log.LogDebug("AutoPilot is not ready.");
                return false;
            }

            // Setup the Waypoint Nav Component
            _waypointNav.Waypoints = waypoints;

            // Start navigation
            Plugin.Log.LogDebug("AutoPilot engaged...");
            AutoPilotInProgress = true;
            _waypointNav.StartNavigation();

            return true;
        }

        /// <summary>
        /// Public method to abort navigation
        /// </summary>
        internal void AbortNavigation()
        {
            _currentAutoPilotState = AutoPilotState.Aborted;
        }
        
        /// <summary>
        /// Handle the state transitions
        /// </summary>
        internal void Update()
        {
            CheckState();
        }

        /// <summary>
        /// Check for changes in status and trigger event
        /// </summary>
        private void CheckState()
        {
            if (_currentAutoPilotState != _previousAutoPilotState)
            {
                _previousAutoPilotState = _currentAutoPilotState;
                StateOrWaypointChanged();
            }
        }

        /// <summary>
        /// Handle a change to Event State or Waypoints
        /// </summary>
        private void StateOrWaypointChanged()
        {
            if (OnAutopilotStatusChanged != null)
            {
                OnAutopilotStatusChanged.Invoke(_currentAutoPilotState, _currentWaypoint);
            }

            switch (_currentAutoPilotState)
            {
                case AutoPilotState.Moving:
                    AutoPilotInProgress = true;
                    break;
                case AutoPilotState.Arrived:
                    AutoPilotInProgress = false;
                    break;
                case AutoPilotState.Aborted:
                    AutoPilotInProgress = false;
                    _waypointNav.StopNavigation();
                    _currentWaypoint = null;
                    _currentAutoPilotState = AutoPilotState.Ready;
                    break;
                default:
                    AutoPilotInProgress = false;
                    break;
            }
        }

        /// <summary>
        /// Handle NavState change event
        /// </summary>
        /// <param name="navState"></param>
        /// <param name="waypoint"></param>
        private void NavStateChangedHandler(NavState navState, Waypoint waypoint)
        {
            Plugin.Log.LogDebug($"AutoPilot.NavStateChangeHandler: {navState}.");
            _currentWaypoint = waypoint;
            _currentNavState = navState;

            // Handle the various SeaTruck states
            switch (_currentNavState)
            {
                case NavState.Moving:
                    _currentAutoPilotState = AutoPilotState.Moving;
                    break;

                case NavState.Arrived:
                    _currentAutoPilotState = AutoPilotState.Arrived;
                    break;

                case NavState.RouteBlocked:
                    _currentAutoPilotState = AutoPilotState.RouteBlocked;
                    break;

                case NavState.WaypointBlocked:
                    _currentAutoPilotState = AutoPilotState.WaypointBlocked;
                    break;

                default:
                    return;
            }
            StateOrWaypointChanged();
        }
    }
}
