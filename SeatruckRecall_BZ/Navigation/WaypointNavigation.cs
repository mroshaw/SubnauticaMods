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
    };

    /// <summary>
    /// WaypointNavigation component, manages moving an object with a rigid body
    /// between a number of Transforms
    /// the game.
    /// </summary>
    internal class WaypointNavigation : MonoBehaviour
    {
        internal class NavStateChangedEvent : UnityEvent<NavState, Waypoint>
        {
        }

        // Public properties
        internal List<Waypoint> Waypoints { get; set; }
        private INavMovement _navMovement;

        // Events to publish current state of Navigation
        internal NavStateChangedEvent OnNavStateChanged = new NavStateChangedEvent();
        internal int StartingWaypointIndex = 0;

        // Internal tracking fields
        private int _currentWaypointIndex;
        private int _totalWaypoints;
        
        // Waypoint tracking
        private Waypoint _previousWaypoint;
        private Waypoint _currentWaypoint;

        // Navigation state
        private NavState _currentNavState = NavState.None;
        private NavState _previousNavState;

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
            _currentNavState = NavState.None;
        }

        /// <summary>
        /// Unity Awake method. Runs every frame so remove this if not required.
        /// Runs frequently, so remove if not required.
        /// </summary>
        internal void Update()
        {
            SetNextState();
            CheckState();
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
        private void SetNextState()
        {
            switch (_currentNavState)
            {
                case NavState.Stopped:
                case NavState.Arrived:
                    return;

                case NavState.WaypointReached:
                    if (!NoMoreWaypoints())
                    {
                        SeaTruckDockRecallPlugin.Log.LogDebug("Moving to next waypoint.");
                        NextWaypoint();
                        _currentNavState = NavState.Moving;
                    }
                    else
                    {
                        SeaTruckDockRecallPlugin.Log.LogDebug("Final Waypoint Reached.");
                        StopNavigation();
                        _currentNavState = NavState.Arrived;
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
            switch (_currentNavState)
            {
                case NavState.Moving:
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
            _currentNavState = NavState.Moving;
            SeaTruckDockRecallPlugin.Log.LogDebug("Starting Waypoint Navigation...");
            _navMovement.PreNavigate();
            NextWaypoint();
        }

        /// <summary>
        /// Public method to pause navigation. Can be restarted
        /// </summary>
        internal void PauseNavigation()
        {
            if (_currentNavState == NavState.Moving)
            {
                _currentNavState = NavState.Paused;
            }
        }

        /// <summary>
        /// Check for changes in status and trigger event
        /// </summary>
        private void CheckState()
        {
            if (_currentNavState != _previousNavState)
            {
                _previousNavState = _currentNavState;
                StateOrWaypointChanged();
            }
        }

        /// <summary>
        /// Handle a change to Event State or Waypoints
        /// </summary>
        private void StateOrWaypointChanged()
        {
            if (OnNavStateChanged != null)
            {
                OnNavStateChanged.Invoke(_currentNavState, _currentWaypoint);
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
            _currentNavState = NavState.Stopped;
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

            // Update waypoint
            _previousWaypoint = _currentWaypoint;
            _currentWaypoint = Waypoints[_currentWaypointIndex];

            // Publish Waypoint change event
            StateOrWaypointChanged();

            // Reset Waypoint nav status
            _currentMoveComplete = false;
            _currentRotateComplete = false;

            /*
            if (!IsWaypointClear(_currentWaypoint, 25.0f))
            {
                _currentNavState = NavState.WaypointBlocked;
                return false;
            }

            if (!IsPathToWaypointClear(gameObject.transform, _currentWaypoint, 25.0f))
            {
                _currentNavState = NavState.RouteBlocked;
                return false;
            }
            */
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
                _currentNavState = NavState.WaypointReached;
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
                SeaTruckDockRecallPlugin.Log.LogInfo($"Found this Collider in waypoint radius: {collider.gameObject.name}");
            }

            /*
            GameObject testSphereGo = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            testSphereGo.name = "WaypointSphere";
            testSphereGo.transform.position = position;
            testSphereGo.transform.localScale = new Vector3(25.0f, 25.0f, 25.0f);
            */
            return true;
        }

        /// <summary>
        /// Uses a SphereCast to see if the path between two waypoints is clear
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        private bool IsPathToWaypointClear(Transform source, Waypoint targetWaypoint, float radius)
        {
            float distance = Vector3.Distance(source.position, targetWaypoint.Transform.position);
            Vector3 direction = (targetWaypoint.Transform.position - source.position).normalized;

            if (Physics.SphereCast(source.position, radius, direction, out RaycastHit hit, distance))
            {
                SeaTruckDockRecallPlugin.Log.LogInfo($"Found this Collider from current position to this Waypoint {targetWaypoint.Name}: {hit.collider.gameObject.name}");
            }

            return true;
        }
    }
}
