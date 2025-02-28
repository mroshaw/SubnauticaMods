using System.Collections.Generic;
using DaftAppleGames.SeatruckRecall_BZ.Navigation;
using UnityEngine;
using UnityEngine.Events;
using static DaftAppleGames.SeatruckRecall_BZ.SeaTruckDockRecallPlugin;

namespace DaftAppleGames.SeatruckRecall_BZ.AutoPilot
{
    // AutoPilot states
    internal enum AutoPilotState
    {
        Idle,
        Ready,
        CalculatingRoute,
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
    internal class AutoPilot : MonoBehaviour
    {
        // Unity Event to publish AutoPilot state changes
        internal class AutopilotStateChangedEvent : UnityEvent<AutoPilotState>
        {
        }

        internal class AutopilotWaypointChangedEvent : UnityEvent<Waypoint>
        {
        }

        // Autopilot state
        public AutoPilotState AutoPilotState => _currentAutoPilotState;
        private AutoPilotState _currentAutoPilotState;
        private AutoPilotState _previousAutoPilotState;
        private NavState _currentNavState;

        // Private fields
        private WaypointNavigation _waypointNav;
        private Waypoint _currentWaypoint;

        // Unity Event publishing Status changes
        internal AutopilotStateChangedEvent OnAutopilotStateChanged = new AutopilotStateChangedEvent();
        internal AutopilotWaypointChangedEvent OnAutopilotWaypointChanged = new AutopilotWaypointChangedEvent();

        protected virtual void OnEnable()
        {
            if (!_waypointNav)
            {
                _waypointNav = GetComponent<WaypointNavigation>();
            }

            // Subscribe to Waypoint changed event
            _waypointNav.OnNavStateChanged.AddListener(NavStateChangedHandler);
            _waypointNav.OnWaypointChanged.AddListener(NavWaypointChangedHandler);

        }

        protected virtual void OnDisable()
        {
            _waypointNav.OnNavStateChanged.RemoveListener(NavStateChangedHandler);
            _waypointNav.OnWaypointChanged.RemoveListener(NavWaypointChangedHandler);
        }

        /// <summary>
        /// Initialise the component
        /// </summary>
        protected virtual void Start()
        {
            // Set default state
            SetAutopilotState(AutoPilotState.Idle);
        }

        internal bool IsAvailable()
        {
            return _currentAutoPilotState == AutoPilotState.Idle;
        }

        /// <summary>
        /// Method to call the Seatruck to the given dock
        /// </summary>
        internal virtual bool BeginNavigation(List<Waypoint> waypoints)
        {
            // Abort, if already being recalled
            if (_currentAutoPilotState != AutoPilotState.Ready)
            {
                // Already being recalled or is already docked
                Log.LogDebug($"AutoPilot BeginNavigation: autopilot is not ready. State is: {_currentAutoPilotState}");
                return false;
            }

            // Setup the Waypoint Nav Component
            _waypointNav.SetWayPoints(waypoints);

            // Start navigation
            Log.LogDebug("AutoPilot engaged!");
            _waypointNav.StartWaypointNavigation();

            return true;
        }

        /// <summary>
        /// Public method to abort navigation
        /// </summary>
        internal void AbortNavigation()
        {
            _waypointNav.StopWaypointNavigation();
            SetAutopilotState(AutoPilotState.Aborted);
        }

        /// <summary>
        /// Used to set the AutoPilot state, and inform listeners
        /// </summary>
        protected void SetAutopilotState(AutoPilotState newState)
        {
            if (_currentAutoPilotState == newState)
            {
                return;
            }
            _currentAutoPilotState = newState;
            OnAutopilotStateChanged?.Invoke(newState);
        }

        /// <summary>
        /// Listen for waypoint changes from the NavMethod and pass it up
        /// </summary>
        private void NavWaypointChangedHandler(Waypoint newWaypoint)
        {
            if (_currentWaypoint == newWaypoint)
            {
                return;
            }

            _currentWaypoint = newWaypoint;
            OnAutopilotWaypointChanged?.Invoke(newWaypoint);
        }

        /// <summary>
        /// Handle NavState change event
        /// </summary>
        private void NavStateChangedHandler(NavState navState)
        {
            Log.LogDebug($"AutoPilot.NavStateChangeHandler: state changed from {_currentNavState} to {navState}");
            _currentNavState = navState;

            // Handle the various SeaTruck states
            switch (_currentNavState)
            {
                case NavState.Moving:
                    SetAutopilotState(AutoPilotState.Moving);
                    break;

                case NavState.Arrived:
                    SetAutopilotState(AutoPilotState.Arrived);
                    SetAutopilotState(AutoPilotState.Ready);
                    break;

                case NavState.RouteBlocked:
                    SetAutopilotState(AutoPilotState.RouteBlocked);
                    break;

                case NavState.WaypointBlocked:
                    SetAutopilotState(AutoPilotState.WaypointBlocked);
                    break;
                default:
                    return;
            }
        }
    }
}