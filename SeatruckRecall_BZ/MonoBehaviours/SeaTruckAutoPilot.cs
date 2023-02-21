using System.Collections.Generic;
using UnityEngine;

namespace DaftAppleGames.SeatruckRecall_BZ.MonoBehaviours
{
    internal enum AutoPilotState
    {
        Ready,
        Moving,
        Arrived,
        Blocked
    };

    internal class SeaTruckAutoPilot : MonoBehaviour
    {
        private SeaTruckMotor _seatruckMotor;
        private Rigidbody _rigidBody;
        private SeaTruckSegment _headSegment;
        private float _moveSpeed;
        internal AutoPilotState AutoPilotState { get; set; }

        private WaypointNavigation _waypointNav;

        /// <summary>
        /// Initialise the component
        /// </summary>
        internal void Start()
        {
            // Get core components
            _seatruckMotor = GetComponent<SeaTruckMotor>();
            _rigidBody = _seatruckMotor.useRigidbody;
            SeaTruckSegment thisSegment = GetComponent<SeaTruckSegment>();
            _headSegment = SeaTruckSegment.GetHead(thisSegment);
            _waypointNav = GetComponent<WaypointNavigation>();

            // Get player config
            _moveSpeed = SeaTruckDockRecallPlugin.TransitSpeed.Value;

            // Set default state
            AutoPilotState = AutoPilotState.Ready;
        }

        /// <summary>
        /// Method to call the Seatruck to the given dock
        /// </summary>
        /// <param name="seaTruckRecaller"></param>
        /// <param name="dockingWaypoints"></param>
        /// <returns></returns>
        internal bool RecallToDock(SeaTruckDockRecaller seaTruckRecaller, List<Waypoint> dockingWaypoints)
        {
            // Abort, if already being recalled
            if (AutoPilotState != AutoPilotState.Ready)
            {
                // Already being recalled or is already docked
                SeaTruckDockRecallPlugin.Log.LogDebug("AutoPilot is not ready.");
                return false;
            }

            // Begin autopilot move to target position

            // Setup the Waypoint Nav Component
            _waypointNav.Waypoints = dockingWaypoints;
            _waypointNav.moveSpeed = _moveSpeed;
            _waypointNav.rigidbody = _rigidBody;
            _waypointNav.SourceTransform = _rigidBody.gameObject.transform;

            // Start navigation
            _waypointNav.StartNavigation();

            return true;
        }

        /// <summary>
        /// Handle the state transitions
        /// </summary>
        internal void Update()
        {
            if (AutoPilotState == AutoPilotState.Ready)
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
                    AutoPilotState = AutoPilotState.Moving;
                    break;

                case NavigationState.FinalWaypointReached:
                    AutoPilotState = AutoPilotState.Arrived;
                    break;

                case NavigationState.Blocked:
                    AutoPilotState = AutoPilotState.Blocked;
                    break;

                default:
                    return;
            }
        }

        /// <summary>
        /// Complete the docking procedure
        /// </summary>
        private void SeaTruckDocking()
        {
            // Collider takes care of docking for us already.
            SeaTruckDockRecallPlugin.Log.LogDebug("Docking complete!");
            AutoPilotState = AutoPilotState.Ready;
        }
    }
}
