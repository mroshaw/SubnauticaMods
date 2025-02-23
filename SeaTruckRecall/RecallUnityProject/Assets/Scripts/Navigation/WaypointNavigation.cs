using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DaftAppleGames.SeatruckRecall_BZ.Navigation
{
    // Manages state of full Waypoint navigation process
    internal enum NavState
    {
        None,
        Idle,
        Moving,
        Paused,
        Stopped,
        WaypointReached,
        Arrived,
        WaypointBlocked,
        RouteBlocked,
    }

    /// <summary>
    /// WaypointNavigation component, manages moving an object with a rigid body
    /// between a number of Transforms
    /// the game.
    /// </summary>
    internal abstract class WaypointNavigation : NavMovement
    {
        internal class NavStateChangedEvent : UnityEvent<NavState>
        {
        }

        internal class NavWaypointChangedEvent : UnityEvent<Waypoint>
        {
        }


        // Events to publish current state of Navigation
        internal NavStateChangedEvent OnNavStateChanged = new NavStateChangedEvent();
        internal NavWaypointChangedEvent OnWaypointChanged = new NavWaypointChangedEvent();

        protected int StartingWaypointIndex { get; set; } = 0;

        private List<Waypoint> _waypoints;

        // Internal tracking fields
        private int _currentWaypointIndex;
        private int _totalWaypoints;

        // Waypoint tracking
        private Waypoint _previousWaypoint;
        private Waypoint _currentWaypoint;

        // Navigation state
        private NavState _currentNavState = NavState.None;
        private NavState _previousNavState;

        private void Start()
        {
            Reset();
        }

        internal void SetWayPoints(List<Waypoint> waypoints)
        {
            _waypoints = waypoints;
            _totalWaypoints = _waypoints.Count;
        }

        /// <summary>
        /// Reset the state to beginning
        /// </summary>
        private void Reset()
        {
            _currentWaypointIndex = StartingWaypointIndex - 1;
            SetWaypoint(null);
            SetNavState(NavState.None);
        }

        /// <summary>
        /// Manage state changes, let listeners know
        /// </summary>
        /// <param name="newState"></param>
        private void SetNavState(NavState newState)
        {
            if (newState == _currentNavState)
            {
                return;
            }

            OnNavStateChanged?.Invoke(newState);
        }

        private void SetWaypoint(Waypoint newWaypoint)
        {
            if (newWaypoint == _currentWaypoint)
            {
                return;
            }

            Log.LogDebug($"Setting new Waypoint: {newWaypoint}");
            _previousWaypoint = _currentWaypoint;
            _currentWaypoint = newWaypoint;
            OnWaypointChanged?.Invoke(newWaypoint);
        }

        /// <summary>
        /// Handles the state transitions
        /// </summary>
        protected override void NavMoveComplete()
        {
            WaypointReached();
            if (!NextWaypoint())
            {
                // If no more waypoints, we've arrived
                SetNavState(NavState.Arrived);
                NavComplete();
            }
        }

        /// <summary>
        /// Override this to execute code before waypoint nav begins
        /// </summary>
        protected internal virtual void NavStarted()
        {
        }

        /// <summary>
        /// Override this to execute code after last waypoint has been reached
        /// </summary>
        protected internal virtual void NavComplete()
        {
        }

        /// <summary>
        /// Override this to execute code before each Waypoint
        /// </summary>
        protected internal virtual void WaypointNavStarted()
        {
        }

        /// <summary>
        /// Override this to execute code after a Waypoint is reached
        /// </summary>
        protected internal virtual void WaypointReached()
        {
        }

        /// <summary>
        /// Public method to pause navigation. Can be restarted
        /// </summary>
        internal void PauseWaypointNavigation()
        {
            if (_currentNavState == NavState.Moving)
            {
                SetNavState(NavState.Paused);
            }
        }

        protected internal virtual void StartWaypointNavigation()
        {
            Reset();
            NavStarted();
            SetNavState(NavState.Moving);
            Log.LogDebug("Starting Waypoint Navigation...");
            NextWaypoint();
        }

        /// <summary>
        /// Public method to restart navigation.
        /// </summary>
        internal void RestartWaypointNavigation()
        {
            if (_currentNavState != NavState.Moving)
            {
                SetNavState(NavState.Moving);
            }
        }

        internal void StopWaypointNavigation()
        {
            if (_currentNavState == NavState.Moving)
            {
                SetNavState(NavState.Stopped);
                NavComplete();
                Reset();
            }
        }

        /// <summary>
        /// Returns true if there are no more waypoints to process
        /// </summary>
        /// <returns></returns>
        private bool IsMoreWaypoints()
        {
            return _currentWaypointIndex < _totalWaypoints;
        }

        /// <summary>
        /// Set the next waypoint, if there is one.
        /// </summary>
        /// <returns></returns>
        private bool NextWaypoint()
        {
            _currentWaypointIndex++;
            if (!IsMoreWaypoints())
            {
                SetWaypoint(null);
                return false;
            }

            // Update waypoint
            SetWaypoint(_waypoints[_currentWaypointIndex]);
            WaypointNavStarted();
            StartNavigation(_currentWaypoint.Transform, _currentWaypoint.RotateBeforeMoving);

            return true;
        }

        /// <summary>
        /// Uses an OverlapSphere to see if there is enough room in front of the dock
        /// for the autopilot to manoeuvre
        /// </summary>
        /// <returns></returns>
        private bool IsWaypointClear(Waypoint waypoint, float radius)
        {
            Vector3 position = waypoint.Transform.position;
            Collider[] allColliders = Physics.OverlapSphere(position, radius);
            foreach (Collider collider in allColliders)
            {
                Log.LogInfo($"Found this Collider in waypoint radius: {collider.gameObject.name}");
            }

            return true;
        }

        /// <summary>
        /// Uses a SphereCast to see if the path between two waypoints is clear
        /// </summary>
        private bool IsPathToWaypointClear(Transform source, Waypoint targetWaypoint, float radius)
        {
            float distance = Vector3.Distance(source.position, targetWaypoint.Transform.position);
            Vector3 direction = (targetWaypoint.Transform.position - source.position).normalized;

            if (Physics.SphereCast(source.position, radius, direction, out RaycastHit hit, distance))
            {
                Log.LogInfo($"Found this Collider from current position to this Waypoint {targetWaypoint.Name}: {hit.collider.gameObject.name}");
            }

            return true;
        }
    }
}