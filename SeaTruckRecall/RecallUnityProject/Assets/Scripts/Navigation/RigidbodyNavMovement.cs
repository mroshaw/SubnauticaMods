using System.Collections.Generic;
using UnityEngine;
using static DaftAppleGames.SeatruckRecall_BZ.SeaTruckDockRecallPlugin;

namespace DaftAppleGames.SeatruckRecall_BZ.Navigation
{
    internal class RigidbodyNavMovement : WaypointNavigation
    {
        // Movement properties for this method of navigation
        [SerializeField] private float rotateSpeed = 50.0f;
        [SerializeField] private float moveSpeed = 10.0f;
        [SerializeField] private float slowDistance = 1.0f;
        [SerializeField] private float slowingAngle = 10f;

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

        protected override UpdateMode GetUpdateMode()
        {
            return UpdateMode.FixedUpdate;
        }

        /// <summary>
        /// Implement the MoveUpdate interface method, using the Rigidbody to move the Source transform
        /// </summary>
        protected override void MoveUpdate(Vector3 targetPosition)
        {
            Vector3 direction = targetPosition - _mainTruckRigidbody.position;
            float distance = direction.magnitude;

            if (distance < 0.01f)
            {
                _mainTruckRigidbody.velocity = Vector3.zero;
                return;
            }

            // Normalize direction for consistent speed
            Vector3 normalizedDirection = direction / distance;

            // Calculate speed scaling: 1 when far, 0 when very close
            float scaleFactor = Mathf.Clamp01(distance / slowDistance);
            float scaledSpeed = moveSpeed * scaleFactor;

            _mainTruckRigidbody.velocity = normalizedDirection * scaledSpeed;
        }

        /// <summary>
        /// Implement the RotateUpdate interface method, using the Rigidbody to move the Source transform
        /// </summary>
        protected override void RotateUpdate(Vector3 targetPosition)
        {
            // Calculate the desired rotation
            Vector3 direction = targetPosition - _mainTruckRigidbody.position;
            if (direction == Vector3.zero) return; // Avoid zero direction
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // Calculate the angle difference
            float angleDiff = Quaternion.Angle(_mainTruckRigidbody.rotation, targetRotation);

            // Scale rotation speed based on how close we are to the target
            float rotateFactor = Mathf.Clamp01(angleDiff / slowingAngle); // 1 = far, 0 = close
            float scaledSpeed = rotateSpeed * rotateFactor;

            // Compute the new rotation with scaled speed
            Quaternion newRotation = Quaternion.RotateTowards(
                _mainTruckRigidbody.rotation,
                targetRotation,
                scaledSpeed * Time.fixedDeltaTime
            );

            // Apply it using physics-friendly method
            _mainTruckRigidbody.MoveRotation(newRotation);
        }

        protected override void WaypointReached()
        {
            base.WaypointReached();
            _mainTruckRigidbody.velocity = Vector3.zero;
        }

        protected override void NavStarted()
        {
            ConfigureRigidBodies();
        }

        /// <summary>
        /// Implement WaypointNavComplete to set the position and reset Rigidbody velocity
        /// </summary>
        protected override void NavComplete()
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
                if (!_rigidBody)
                {
                    Log.LogDebug("Backup RigidBody is null!");
                    return;
                }
                _rigidBody.drag = 0;
                _rigidBody.mass = 0;
                UWE.Utils.SetIsKinematicAndUpdateInterpolation(_rigidBody, true, false);
            }

            internal void Restore()
            {
                if (!_rigidBody)
                {
                    Log.LogDebug("Backup RigidBody is null!");
                    return;
                }
                _rigidBody.mass = _mass;
                _rigidBody.drag = _drag;
                UWE.Utils.SetIsKinematicAndUpdateInterpolation(_rigidBody, false, true);
            }
        }
    }
}