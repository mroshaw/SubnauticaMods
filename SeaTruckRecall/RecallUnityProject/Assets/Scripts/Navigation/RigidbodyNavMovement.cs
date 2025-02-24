using System.Collections.Generic;
using UnityEngine;

namespace DaftAppleGames.SeatruckRecall_BZ.Navigation
{
    internal class RigidbodyNavMovement : WaypointNavigation
    {
        // Movement properties for this method of navigation
        protected override float RotateSpeed => 25.0f;
        protected override float MoveSpeed => 10.0f;
        protected override float RotateThreshold => 0.5f;

        // Private fields
        private Vector3 _directionToTarget;
        private Vector3 _currentDirection;
        private float _distanceToTarget;

        private float _stoppingDistance = 5.0f;
        private float _moveForce = 1000.0f;

        private Rigidbody _mainTruckRigidbody;
        private List<RigidBodyBackup> _rigidBodyBackups;
        private int _numChildRigidBodies;

        /// <summary>
        /// Initialise the component
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            InitRigidBodies();
        }

        private void InitRigidBodies()
        {
            _rigidBodyBackups = new List<RigidBodyBackup>();

            _mainTruckRigidbody = GetComponent<Rigidbody>();
            Rigidbody[] allRigidBodies = gameObject.GetComponentsInChildren<Rigidbody>(true);

            for (int curRb = 0; curRb < allRigidBodies.Length; curRb++)
            {
                if (allRigidBodies[curRb] != _mainTruckRigidbody)
                {
                    _rigidBodyBackups.Add(new RigidBodyBackup(allRigidBodies[curRb]));
                }
            }

            _numChildRigidBodies = _rigidBodyBackups.Count;
        }

        /// <summary>
        /// Implement the MoveUpdate interface method, using the Rigidbody to move the Source transform
        /// </summary>
        protected override void MoveUpdate(Vector3 targetPosition)
        {
            Vector3 force = ComputeMovementForce(targetPosition);
            _mainTruckRigidbody.AddForce(force, ForceMode.Force);
        }

        /// <summary>
        /// Implement the RotateUpdate interface method, using the Rigidbody to move the Source transform
        /// </summary>
        protected override void RotateUpdate(Vector3 targetPosition)
        {
            Vector3 torque = ComputeRotationTorque(targetPosition);
            _mainTruckRigidbody.AddTorque(torque, ForceMode.Impulse);
        }

        protected internal override void WaypointReached()
        {
            base.WaypointReached();
            _mainTruckRigidbody.velocity = Vector3.zero;
        }

        protected internal override void NavStarted()
        {
            ConfigureRigidBodies();
        }

        /// <summary>
        /// Implement WaypointNavComplete to set the position and reset Rigidbody velocity
        /// </summary>
        protected internal override void NavComplete()
        {
            _mainTruckRigidbody.angularVelocity = Vector3.zero;
            _mainTruckRigidbody.velocity = Vector3.zero;
            RestoreRigidBodies();
        }

        /// <summary>
        /// Configure all SeaTruck module rigidbodies
        /// </summary>
        public void RestoreRigidBodies()
        {
            Log.LogDebug($"RestoreChildRigidBodies {_numChildRigidBodies}child RigidBodies");
            foreach (RigidBodyBackup backup in _rigidBodyBackups)
            {
                backup.Restore();
            }
            UWE.Utils.SetIsKinematicAndUpdateInterpolation(_mainTruckRigidbody, true, true);
        }

        /// <summary>
        /// Set drag and mass to zero of all child Rigidbodies
        /// </summary>
        public void ConfigureRigidBodies()
        {
            Log.LogDebug($"ZeroChildRigidBodies: {_numChildRigidBodies} child RigidBodies");
            foreach (RigidBodyBackup backup in _rigidBodyBackups)
            {
                backup.Zero();
            }
            UWE.Utils.SetIsKinematicAndUpdateInterpolation(_mainTruckRigidbody, false, false);
        }

        private Vector3 ComputeMovementForce(Vector3 targetPosition)
        {
            _directionToTarget = (targetPosition - transform.position);
            _distanceToTarget = _directionToTarget.magnitude;

            // Normalize direction
            _directionToTarget.Normalize();

            // Scale force based on distance (slows down when close)
            float speedFactor = Mathf.Clamp01(_distanceToTarget / _stoppingDistance);
            Vector3 desiredVelocity = _directionToTarget * speedFactor * MoveSpeed;

            // Compute required force to reach the desired velocity
            Vector3 velocityChange = desiredVelocity - _mainTruckRigidbody.velocity;
            Vector3 force = velocityChange * _moveForce;

            return force;

        }

        /// <summary>
        /// Calculate the amount of torque to apply to rotate
        /// between current and target transforms
        /// </summary>
        private Vector3 ComputeRotationTorque(Vector3 targetPosition)
        {
            // Compute direction to target
            Vector3 direction = targetPosition - transform.position;
            if (direction.sqrMagnitude < Mathf.Epsilon) return Vector3.zero; // Prevent errors

            // Compute the desired rotation
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // Get the rotation difference
            Quaternion rotationDifference = targetRotation * Quaternion.Inverse(transform.rotation);

            // Convert quaternion difference to axis-angle representation
            rotationDifference.ToAngleAxis(out float angle, out Vector3 axis);

            if (angle > 180f)
            {
                angle -= 360f; // Normalize angle range (-180 to 180)
            }

            // Calculate torque
            Vector3 torque = axis * (RotateSpeed * Mathf.Deg2Rad * angle); // Convert angle to radians

            return torque;
        }

        internal class RigidBodyBackup
        {
            private Rigidbody _rigidBody;
            private float _mass;
            private float _drag;

            internal RigidBodyBackup(Rigidbody rigidBody)
            {
                _rigidBody = rigidBody;
                _mass = rigidBody.mass;
                _drag = rigidBody.drag;
            }

            internal void Zero()
            {

                _rigidBody.drag = 0;
                _rigidBody.mass = 0;
                UWE.Utils.SetIsKinematicAndUpdateInterpolation(_rigidBody, true, false);

            }

            internal void Restore()
            {
                _rigidBody.mass = _mass;
                _rigidBody.drag = _drag;
                UWE.Utils.SetIsKinematicAndUpdateInterpolation(_rigidBody, false, true);
            }
        }
    }
}