using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DaftAppleGames.SeatruckRecall_BZ.Navigation
{
    // Manages state of full Waypoint navigation process
    internal enum NavigationState
    {
        Stopped,
        Blocked,
        Paused,
        Moving,
        WaypointReached,
        FinalWaypointReached
    };

    /// <summary>
    /// WaypointNavigation component, manages moving an object with a rigid body
    /// between a number of Transforms
    /// the game.
    /// </summary>
    internal class WaypointNavigation : MonoBehaviour
    {
        // Unity Event class to publish Waypoint changes
        internal class WaypointChangedEvent : UnityEvent<Waypoint>
        {
        }

        // Public properties
        internal List<Waypoint> Waypoints { get; set; }
        private INavMovement _navMovement;

        // Event to subscribe
        internal WaypointChangedEvent OnWaypointChangedEvent = new WaypointChangedEvent();
        internal int StartingWaypointIndex = 0;

        // Internal tracking fields
        private int _currentWaypointIndex;
        private int _totalWaypoints;
        
        // Current Waypoint
        private Waypoint _currentWaypoint;
        internal NavigationState NavState { get; set; }
        private bool _currentMoveComplete;
        private bool _currentRotateComplete;

        /// <summary>
        /// Unity Start method
        /// </summary>
        public void Start()
        {
            // Get the NavMovement component
            _navMovement = GetComponent<INavMovement>();

            // Configure the NavMovement component
            _navMovement.RotateSpeed = SeaTruckDockRecallPlugin.RotationSpeed.Value;
            _navMovement.MoveSpeed = SeaTruckDockRecallPlugin.TransitSpeed.Value;
            _navMovement.MoveTolerance = 0.2f;
            _navMovement.RotateTolerance = 0.99f;
            _navMovement.SlowDistance = 2.0f;

            ResetState();
        }

        /// <summary>
        /// Reset the state to beginning
        /// </summary>
        private void ResetState()
        {
            _currentWaypointIndex = 0;
            NavState = NavigationState.Stopped;
        }

        /// <summary>
        /// Unity Awake method. Runs every frame so remove this if not required.
        /// Runs frequently, so remove if not required.
        /// </summary>
        internal void Update()
        {
            ManageStateUpdate();
        }

        /// <summary>
        /// Unity Physics Update (FixedUpdate) method.
        /// Runs frequently, so remove if not required.
        /// </summary>
        internal void FixedUpdate()
        {
            ManageMoveUpdate();
        }

        /// <summary>
        /// Handles the state transitions
        /// </summary>
        private void ManageStateUpdate()
        {
            switch (NavState)
            {
                case NavigationState.Stopped:
                case NavigationState.FinalWaypointReached:
                    return;

                case NavigationState.WaypointReached:
                    if (!NoMoreWaypoints())
                    {
                        SeaTruckDockRecallPlugin.Log.LogDebug("Moving to next waypoint.");
                        NextWaypoint();
                        NavState = NavigationState.Moving;
                    }
                    else
                    {
                        SeaTruckDockRecallPlugin.Log.LogDebug("Final Waypoint Reached.");
                        NavState = NavigationState.FinalWaypointReached;
                        StopNavigation();
                    }
                    break;

                default:
                    return;
            }
        }

        /// <summary>
        /// Manage Move state functions in 
        /// </summary>
        private void ManageMoveUpdate()
        {
            switch (NavState)
            {
                case NavigationState.Moving:
                    MoveToWaypointUpdate();
                    break;

                default:
                    return;
            }
        }

        /// <summary>
        /// Public method to start navigating waypoint list
        /// </summary>
        internal void StartNavigation()
        {
            ResetState();
            _totalWaypoints = Waypoints.Count;
            _currentWaypointIndex = StartingWaypointIndex -1;
            NavState = NavigationState.Moving;
            _navMovement.PreNavigate();
            NextWaypoint();
            SeaTruckDockRecallPlugin.Log.LogDebug("Starting Waypoint Navigation...");
        }

        /// <summary>
        /// Public method to pause navigation. Can be restarted
        /// </summary>
        internal void PauseNavigation()
        {
            if (NavState == NavigationState.Moving)
            {
                NavState = NavigationState.Paused;
            }
        }

        /// <summary>
        /// Public method to pause navigation. Can be restarted
        /// </summary>
        internal void RestartNavigation()
        {
            StartNavigation();
        }

        internal void StopNavigation()
        {
            ResetState();
            NavState = NavigationState.Stopped;
            _navMovement.PostNavigate();
        }

        /// <summary>
        /// Returns true if there are no more waypoints to process
        /// </summary>
        /// <returns></returns>
        private bool NoMoreWaypoints()
        {
            return _currentWaypointIndex == _totalWaypoints;
        }

        /// <summary>
        /// Set the next waypoint, if there is one.
        /// </summary>
        /// <returns></returns>
        private bool NextWaypoint()
        {
            _currentWaypointIndex++;
            if (NoMoreWaypoints())
            {
                return false;
            }

            _currentWaypoint = Waypoints[_currentWaypointIndex];
            OnWaypointChangedEvent.Invoke(_currentWaypoint);
            _currentMoveComplete = false;
            _currentRotateComplete = false;
            return true;
        }

        /// <summary>
        /// Physics update to move and rotate the transform towards the target
        /// </summary>
        private void MoveToWaypointUpdate()
        {
            if (_currentMoveComplete && _currentRotateComplete)
            {
                SeaTruckDockRecallPlugin.Log.LogDebug($"Waypoint {_currentWaypointIndex} complete.");
                NavState = NavigationState.WaypointReached;
            }

            // Move to current waypoint
            // Wait until rotation is complete first, if that's the current Waypoint action
            if (!_currentMoveComplete && (_currentWaypoint.Action == WaypointAction.RotateOnMove ||
                                          _currentWaypoint.Action == WaypointAction.RotateBeforeMove && _currentRotateComplete))
            {
                MoveUpdate();
            }

            // Rotate towards waypoint
            if (!_currentRotateComplete)
            {
                RotateUpdate();
            }
        }

        /// <summary>
        /// Handles the physics movement to the current waypoint
        /// </summary>
        private void MoveUpdate()
        {
            if (_currentMoveComplete)
            {
                return;
            }

            // Move and check if we've arrived
            if (_navMovement.MoveUpdate(_currentWaypoint.Transform))
            {
                SeaTruckDockRecallPlugin.Log.LogDebug($"Waypoint {_currentWaypointIndex} reached.");
                _currentMoveComplete = true;
            }
        }

        /// <summary>
        /// Handles the physics rotation to current Waypoint
        /// </summary>
        private void RotateUpdate()
        {
            if (_currentRotateComplete)
            {
                return;
            }

            // Rotate and check if we're now facing the target
            if (_navMovement.RotateUpdate(_currentWaypoint.Transform))
            {
                SeaTruckDockRecallPlugin.Log.LogDebug($"Waypoint {_currentWaypointIndex} rotation complete.");
                _currentRotateComplete = true;
            }
        }
    }
}
