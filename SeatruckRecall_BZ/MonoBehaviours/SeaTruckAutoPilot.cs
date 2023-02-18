using UnityEngine;
using static RootMotion.FinalIK.RagdollUtility;

namespace DaftAppleGames.SeatruckRecall_BZ.MonoBehaviours
{
    internal class SeaTruckAutoPilot : MonoBehaviour
    {
        private SeaTruckMotor _seatruckMotor;
        private Rigidbody _rigidBody;
        private SeaTruckSegment _headSegment;
        private float _transitSpeed;

        private Vector3 _currentTarget;
        private SeaTruckDockRecaller _currentDockRecallerTarget;
        private SeaTruckRecallStatus _latestRecallStatus = SeaTruckRecallStatus.None;

        /// <summary>
        /// Initialise the component
        /// </summary>
        public void Start()
        {
            _seatruckMotor = GetComponent<SeaTruckMotor>();
            _rigidBody = _seatruckMotor.useRigidbody;
            SeaTruckSegment thisSegment = GetComponent<SeaTruckSegment>();
            _headSegment = SeaTruckSegment.GetHead(thisSegment);
            _transitSpeed = SeaTruckDockRecallPlugin.TransitSpeed.Value;
        }

        /// <summary>
        /// Method to call the Seatruck to the given dock
        /// </summary>
        /// <param name="seaTruckRecaller"></param>
        /// <returns></returns>
        public bool RecallToDock(SeaTruckDockRecaller seaTruckRecaller, Vector3 targetPosition)
        {
            // Abort, if already being recalled
            if (_latestRecallStatus == SeaTruckRecallStatus.InTransit || _latestRecallStatus == SeaTruckRecallStatus.Docking)
            {
                // Already being recalled
                SeaTruckDockRecallPlugin.Log.LogDebug("SeaTruck already being recalled.");
                return false;
            }

            // Begin autopilot move to target position
            _currentTarget = targetPosition;
            _currentDockRecallerTarget = seaTruckRecaller;
            _latestRecallStatus = SeaTruckRecallStatus.InTransit;
            return true;
        }

        /// <summary>
        /// Handle the state transitions
        /// </summary>
        public void Update()
        {
            // Handle the various SeaTruck states
            switch (_latestRecallStatus)
            {
                // If docked or idle, do nothing
                case SeaTruckRecallStatus.None:
                case SeaTruckRecallStatus.Docked:
                    return;

                // If ArrivedAtDock, rotate into docking position
                case SeaTruckRecallStatus.ArrivedAtDock:
                    SeaTruckArrived();
                    break;

                // If InPosition, trigger the docking mechanism
                case SeaTruckRecallStatus.InPosition:
                    SeaTruckInPosition();
                    break;

                default:
                    return;
            }
        }

        /// <summary>
        /// Handle SeaTruck movement states
        /// </summary>
        public void FixedUpdate()
        {
            // Handle the various SeaTruck states
            switch (_latestRecallStatus)
            {
                // If docked or idle, do nothing
                case SeaTruckRecallStatus.None:
                case SeaTruckRecallStatus.Docked:
                    return;

                // If InTransit, move towards target
                case SeaTruckRecallStatus.InTransit:
                    SeaTruckTransit();
                    break;

                // If RotateToPosition, rotate the SeaTruck
                case SeaTruckRecallStatus.RotatingToPosition:
                    SeaTruckRotate();
                    break;

                // If Docking, move towards docking mechanism
                case SeaTruckRecallStatus.Docking:
                    SeaTruckDocking();
                    break;

                default:
                    return;
            }
        }

        /// <summary>
        /// Handles moving the SeaTruck to target coordinates
        /// </summary>
        private void SeaTruckTransit()
        {
            // Check to see if we've arrived
            if (Vector3.Distance(transform.position, _currentTarget) < 0.2f)
            {
                SeaTruckDockRecallPlugin.Log.LogDebug("SeaTruck has arrived at dock.");
                _latestRecallStatus = SeaTruckRecallStatus.ArrivedAtDock;
                return;
            }

            // Move the Seatruck towards the target
            transform.LookAt(_currentTarget);
            _rigidBody.AddRelativeForce(Vector3.forward * _transitSpeed, ForceMode.Force);
        }

        /// <summary>
        /// Handles SeaTruck arrival at the dock location
        /// </summary>
        private void SeaTruckArrived()
        {
            SeaTruckDockRecallPlugin.Log.LogDebug("SeaTruck arrived!");
            // Rotate to position
            _latestRecallStatus = SeaTruckRecallStatus.RotatingToPosition;
        }

        /// <summary>
        /// Rotate the SeaTruck to face the Docking tube
        /// </summary>
        private void SeaTruckRotate()
        {
            // TODO

            // When in position, set ready to dock
            _latestRecallStatus = SeaTruckRecallStatus.InPosition;
        }

        /// <summary>
        /// SeaTruck is in position, begin docking
        /// </summary>
        private void SeaTruckInPosition()
        {
            _latestRecallStatus = SeaTruckRecallStatus.Docking;
        }

        /// <summary>
        /// Move SeaTruck to docking trigger
        /// </summary>
        private void SeaTruckDocking()
        {
            // TODO
            // When docked, we're done!
            _latestRecallStatus = SeaTruckRecallStatus.Docked;
        }
    }
}
