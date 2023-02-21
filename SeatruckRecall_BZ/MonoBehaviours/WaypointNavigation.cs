using System.Collections.Generic;
using UnityEngine;

namespace DaftAppleGames.SeatruckRecall_BZ.MonoBehaviours
{
    // Manages state of full Waypoint navigation process
    internal enum NavigationState {Stopped, Blocked, Paused, Moving, WaypointReached, FinalWaypointReached}

    /// <summary>
    /// WaypointNavigation component, manages moving an object with a rigid body
    /// between a number of Transforms
    /// the game.
    /// </summary>
    internal class WaypointNavigation : MonoBehaviour
    {
        // Public properties
        internal List<Waypoint> Waypoints;
        internal Transform SourceTransform;

        internal float moveSpeed;
        internal float rotateSpeed;
        internal float slowDistance = 2.0f;
        internal float moveTolerance = 0.2f;
        internal float rotateTolerance = 0.99f;
        internal int startingWaypointIndex = 0;

        internal Rigidbody rigidbody;

        // Internal tracking attributes
        private int _currentWaypointIndex;
        private int _totalWaypoints;

        private float _distanceToCurrentTarget;
        private Vector3 _currentDirection;

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
            rotateSpeed = SeaTruckDockRecallPlugin.RotationSpeed.Value;
            moveSpeed = SeaTruckDockRecallPlugin.TransitSpeed.Value;

            ResetState();
        }

        /// <summary>
        /// Reset the state to beginning
        /// </summary>
        private void ResetState()
        {
            _currentWaypointIndex = startingWaypointIndex;
            _distanceToCurrentTarget = 0.0f;
            NavState = NavigationState.Stopped;

            // If no transform specific, assume current
            if (!SourceTransform)
            {
                SourceTransform = transform;
            }

            rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
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
                    if (!HasReachedFinalWaypoint())
                    {
                        SeaTruckDockRecallPlugin.Log.LogDebug("Moving to next waypoint.");
                        NextWaypoint();
                        NavState = NavigationState.Moving;
                    }
                    else
                    {
                        SeaTruckDockRecallPlugin.Log.LogDebug("Final Waypoint Reached.");
                        NavState = NavigationState.FinalWaypointReached;
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
            _currentWaypoint = Waypoints[startingWaypointIndex];
            NavState = NavigationState.Moving;
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
            if (NavState == NavigationState.Moving)
            {
                NavState = NavigationState.Moving;
            }
        }

        internal void StopNavigation()
        {
            NavState = NavigationState.Stopped;
        }

        /// <summary>
        /// Returns true if within buffer range of target waypoint
        /// </summary>
        /// <returns></returns>
        private bool HasArrived()
        {
            _distanceToCurrentTarget = Vector3.Distance(SourceTransform.position, _currentWaypoint.Transform.position);
            return _distanceToCurrentTarget < moveTolerance;
        }

        /// <summary>
        /// Returns true if within range of target rotation
        /// </summary>
        /// <returns></returns>
        private bool HasRotated()
        {
            Vector3 dirToTarget = (_currentWaypoint.Transform.position - SourceTransform.position).normalized;
            float dotProduct = Vector3.Dot(dirToTarget, SourceTransform.forward);

            // When in position, set ready to dock
             return dotProduct > rotateTolerance;
        }

        /// <summary>
        /// Returns true if there are no more waypoints to process
        /// </summary>
        /// <returns></returns>
        private bool HasReachedFinalWaypoint()
        {
            return _currentWaypointIndex + 1 == _totalWaypoints;
        }

        /// <summary>
        /// Set the next waypoint, if there is one.
        /// </summary>
        /// <returns></returns>
        private bool NextWaypoint()
        {
            if (!HasReachedFinalWaypoint())
            {
                _currentWaypointIndex++;
                _currentWaypoint = Waypoints[_currentWaypointIndex];
                _currentMoveComplete = false;
                _currentRotateComplete = false;
                return true;
            }
            return false;
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

            // Check to see if we've arrived
            if (HasArrived())
            {
                SeaTruckDockRecallPlugin.Log.LogDebug($"Waypoint {_currentWaypointIndex} reached.");
                rigidbody.angularVelocity = Vector3.zero;
                rigidbody.velocity = Vector3.zero;
                SourceTransform.position = _currentWaypoint.Transform.position;
                _currentMoveComplete = true;
                return;
            }

            _currentDirection = (_currentWaypoint.Transform.position - SourceTransform.position).normalized;

            if (_distanceToCurrentTarget > slowDistance)
            {
                rigidbody.velocity = _currentDirection * moveSpeed;
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

            if (HasRotated())
            {
                SeaTruckDockRecallPlugin.Log.LogDebug($"Waypoint {_currentWaypointIndex} rotation complete.");
                rigidbody.angularVelocity = Vector3.zero;
                rigidbody.velocity = Vector3.zero;
                SourceTransform.LookAt(_currentWaypoint.Transform.position);
                _currentRotateComplete = true;
                return;
            }

            // Apply rigid body rotation
            RotateByTorque(SourceTransform, _currentWaypoint.Transform, rotateSpeed);
        }

        /// <summary>
        /// Rotate the rigid body using torque
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="rotationTorque"></param>
        private void RotateByTorque(Transform source, Transform target, float rotationTorque)
        {
            Vector3 torque = ComputeTorque(source, target, rotationTorque);
            rigidbody.AddTorque(torque, ForceMode.Impulse);
        }

        /// <summary>
        /// Rotate the rigid body using MoveRotation
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="rotSpeed"></param>
        private void RotateByMove(Transform source, Transform target, float rotSpeed)
        {
            Vector3 targetDirection = target.position - source.position;
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            Quaternion deltaRotation = Quaternion.RotateTowards(rigidbody.rotation, targetRotation, rotSpeed * Time.deltaTime);

            rigidbody.MoveRotation(deltaRotation);
        }

        /// <summary>
        /// Calculate the amount of torque to apply to rotate
        /// between current and target transforms
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="rotSpeed"></param>
        /// <returns></returns>
        private Vector3 ComputeTorque(Transform source, Transform target, float rotSpeed)
        {
            Vector3 targetDirection = target.position - source.position;
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

            // Rotate from our current rotation to desired rotation
            Quaternion deltaRotation = targetRotation * Quaternion.Inverse(source.rotation);

            // Convert to angle axis representation so we can do math with angular velocity
            Vector3 angleAxis;
            deltaRotation.ToAngleAxis(out float axisMagnitude, out angleAxis);
            angleAxis.Normalize();

            // Angular velocity we need to achieve
            Vector3 angluarVelocity = angleAxis * axisMagnitude * Mathf.Deg2Rad / Time.fixedDeltaTime * 0.5f;
            angluarVelocity -= rigidbody.angularVelocity;

            // to multiply with inertia tensor local then rotationTensor coords
            Vector3 transformDirection = source.InverseTransformDirection(angluarVelocity);
            Vector3 transformRotation;
            Vector3 tensorRotation = transformDirection;
            tensorRotation = rigidbody.inertiaTensorRotation * tensorRotation;
            tensorRotation.Scale(rigidbody.inertiaTensor);
            transformRotation = Quaternion.Inverse(rigidbody.inertiaTensorRotation) * tensorRotation;
            Vector3 torqueToApply = source.TransformDirection(transformRotation);
            return torqueToApply;
        }
    }
}
