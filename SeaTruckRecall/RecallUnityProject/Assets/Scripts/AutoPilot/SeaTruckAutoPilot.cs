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
        Ready,
        CalculatingRoute,
        Moving,
        Aborted,
        Arrived,
        Blocked,
        Parking,
        Docked
    };


    /// <summary>
    /// MonoBehavior implementing SeaTruck specific AutoPilot behavior
    /// the game.
    /// </summary>
    internal class SeaTruckAutoPilot : MonoBehaviour
    {

        // Unity Event to publish AutoPilot state changes
        internal class AutopilotStateChangedEvent : UnityEvent<AutoPilotState>
        {
        }

        [SerializeField] private float pathCellSize = 10.0f;
        [SerializeField] private int pathCellExtends = 5;
        [SerializeField] private List<Waypoint> currentWaypoints;

        // Autopilot state
        private AutoPilotState _currentAutoPilotState;
        private NavState _currentNavState;

        // Stuck settings

        // If the autopilot is active and doesn't "move" for this amount of time and distance, it's considered "stuck"
        private const float StuckCheckTimeThreshold = 10.0f;
        private const float StuckCheckPositionThreshold = 0.1f;
        private const float StuckCheckRotationThreshold = 5.0f;

        private float _currStuckCheckTimer = 0.0f;

        private Vector3 _lastPosition =  Vector3.zero;
        private Quaternion _lastRotation = Quaternion.identity;

        // Component references
        private WaypointNavigation _waypointNav;
        private InstantNavigation _instantNav;
        private Waypoint _currentWaypoint;

        // Unity Event publishing Status changes
        internal AutopilotStateChangedEvent OnAutopilotStateChanged = new AutopilotStateChangedEvent();
        internal WaypointChangedEvent OnAutopilotWaypointChanged = new WaypointChangedEvent();

        private PathFinder _pathFinder;
        private List<Waypoint> _recallWaypoints;

        // Lists the layers to INCLUDE in collision avoidance
        private LayerMask _collisionLayerMask;
        private Transform _debugContainer;

        protected virtual void OnEnable()
        {
            InitComponentReferences();
            if (_waypointNav)
            {
                // Subscribe to Waypoint changed event
                _waypointNav.OnNavStateChanged.AddListener(NavStateChangedHandler);
                _waypointNav.OnWaypointChanged.AddListener(NavWaypointChangedHandler);
            }
        }

        protected virtual void OnDisable()
        {
            InitComponentReferences();
            if (_waypointNav)
            {
                _waypointNav.OnNavStateChanged.RemoveListener(NavStateChangedHandler);
                _waypointNav.OnWaypointChanged.RemoveListener(NavWaypointChangedHandler);
            }
        }

        private void Awake()
        {
            if (!_pathFinder)
            {
                _pathFinder = GetComponent<PathFinder>();
            }
            // Include layers for obstacle detection
            _collisionLayerMask = LayerMask.GetMask("TerrainCollider", "Default");
        }

        private void Start()
        {
            // Set default state
            SetAutopilotState(AutoPilotState.Ready);

            GameObject debugGameObject = new GameObject("NAVGRID");
            _debugContainer = debugGameObject.transform;
        }

        private void Update()
        {
            if (_currentAutoPilotState != AutoPilotState.Moving)
            {
                return;
            }

            // If we get stuck, change status and notify listeners
            if (IsStuckCheck())
            {
                Log.LogDebug("AutoPilot is stuck! Aborting!");
                AbortNavigation();
            }
        }

        public bool StartNavigation(List<Waypoint> waypoints)
        {
            // Generate a path from the SeaTruck to the first point in the Recaller waypoints
            Vector3 targetPosition = waypoints[0].Position;

            _recallWaypoints = waypoints;

            // Creates a grid of cells 10.0 units square, 5 squares to the left, right, top and bottom from the SeaTruck to the target.
            SetAutopilotState(AutoPilotState.CalculatingRoute);
            _pathFinder.GenerateWaypoints(transform.position, targetPosition, pathCellSize, pathCellExtends, _collisionLayerMask, null, null, WaypointsCompleteHandler, true, _debugContainer);

            return true;
        }

        public void BeginParking()
        {
            SetAutopilotState(AutoPilotState.Parking);
        }

        public void DockingComplete()
        {
            SetAutopilotState(AutoPilotState.Docked);
        }

        public void ReleaseFromDock()
        {
            SetAutopilotState(AutoPilotState.Ready);
        }

        private void InitComponentReferences()
        {
            if (!_waypointNav)
            {
                _waypointNav = GetComponent<WaypointNavigation>();
            }

            if (!_instantNav)
            {
                _instantNav = GetComponent<InstantNavigation>();
            }
        }

        /// <summary>
        /// Manages the Pathing responses from the PathFinder MonoBehaviour
        /// </summary>
        private void WaypointsCompleteHandler(GenerateStatus status, List<Waypoint> pathWaypoints)
        {
            // Confirm we can get the waypoints from the PathFinder
            if (status == GenerateStatus.Success)
            {
                currentWaypoints = pathWaypoints;

                // Append the remaining dock recall waypoints
                foreach (Waypoint currWayPoint in _recallWaypoints)
                {
                    currentWaypoints.Add(currWayPoint);
                }

                // Begin moving through the waypoints
                SetAutopilotState(AutoPilotState.Ready);
                NavigateWaypoints(currentWaypoints);
            }
        }

        private bool IsStuckCheck()
        {
            _currStuckCheckTimer += Time.deltaTime;

            if (_currStuckCheckTimer < StuckCheckTimeThreshold)
            {
                return false;
            }

            // Reset timer
            _currStuckCheckTimer = 0.0f;

            if (_lastPosition.Equals(Vector3.zero) || _lastRotation.Equals(Quaternion.identity))
            {
                _lastPosition = transform.position;
                _lastRotation = transform.rotation;
                return false;
            }

            // Check how far we've travelled and rotated since the last stuck check
            if (Vector3.Distance(_lastPosition, transform.position) < StuckCheckPositionThreshold &&
                Quaternion.Angle(_lastRotation, transform.rotation) < StuckCheckRotationThreshold)
            {
                return true;
            }

            // Reset position
            _lastPosition = transform.position;
            _lastRotation = transform.rotation;
            return false;

        }

        internal bool IsAvailable()
        {
            return _currentAutoPilotState == AutoPilotState.Ready;
        }

        /// <summary>
        /// Begin navigating to the list of waypoints given
        /// </summary>
        private bool NavigateWaypoints(List<Waypoint> waypoints)
        {
            // Abort, if already being recalled
            if (_currentAutoPilotState != AutoPilotState.Ready)
            {
                // Already being recalled or is already docked
                Log.LogDebug($"AutoPilot BeginNavigation: autopilot is not ready. State is: {_currentAutoPilotState}");
                return false;
            }

            if (_instantNav)
            {
                SetAutopilotState(AutoPilotState.Moving);
                _instantNav.MoveToDestination(waypoints);
                SetAutopilotState(AutoPilotState.Arrived);
                return true;
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
        private void AbortNavigation()
        {
            _waypointNav.StopWaypointNavigation();
            SetAutopilotState(AutoPilotState.Aborted);
        }

        /// <summary>
        /// Used to set the AutoPilot state, and inform listeners
        /// </summary>
        private void SetAutopilotState(AutoPilotState newState)
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
                case NavState.WaypointBlocked:
                case NavState.RouteBlocked:
                    SetAutopilotState(AutoPilotState.Blocked);
                    break;
                default:
                    return;
            }
        }
    }
}