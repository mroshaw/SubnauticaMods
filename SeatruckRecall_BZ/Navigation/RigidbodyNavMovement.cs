using System;
using UnityEngine;
using static DaftAppleGames.SeatruckRecall_BZ.SeaTruckDockRecallPlugin;

namespace DaftAppleGames.SeatruckRecall_BZ.Navigation
{
    internal class RigidbodyNavMovement : WaypointNavigation
    {
        // Private fields
        private Vector3 _currentDirection;
        private float _distanceToTarget;
        private Rigidbody _sourceRigidbody;

        // Backup of child rigidbody values
        private float[] _backupMass;
        private float[] _backupDrag;

        /// <summary>
        /// Initialise the component
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            _sourceRigidbody = gameObject.GetComponentInChildren<Rigidbody>();
            _sourceRigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        }

        /// <summary>
        /// Implement the MoveUpdate interface method, using the Rigidbody to move the Source transform
        /// </summary>
        protected override void MoveUpdate(Transform targetTransform)
        {
            _distanceToTarget = Vector3.Distance(targetTransform.position, targetTransform.position);
            _currentDirection = (targetTransform.position - targetTransform.position).normalized;
            if (_distanceToTarget > SlowDistance)
            {
                _sourceRigidbody.velocity = _currentDirection * MoveSpeed;
            }
        }

        /// <summary>
        /// Implement the RotateUpdate interface method, using the Rigidbody to move the Source transform
        /// </summary>
        protected override void RotateUpdate(Transform targetTransform)
        {
            Vector3 torque = ComputeRotationTorque(targetTransform);
            _sourceRigidbody.AddTorque(torque, ForceMode.Impulse);
        }

        protected internal override void StartWaypointNavigation(Action preNavigateDelegate = null)
        {
            base.StartWaypointNavigation(ConfigureRigidBodies);
        }

        /// <summary>
        /// Implement WaypointNavComplete to set the position and reset Rigidbody velocity
        /// </summary>
        protected override void WaypointNavComplete()
        {
            _sourceRigidbody.angularVelocity = Vector3.zero;
            _sourceRigidbody.velocity = Vector3.zero;
            ResetRigidBodies();
        }

        /// <summary>
        /// Configure all SeaTruck module rigidbodies
        /// </summary>
        private void ConfigureRigidBodies()
        {
            Log.LogDebug("RigidBodyNavMovement PostNavigate");
            Rigidbody[] allRigidBodies = gameObject.GetComponentsInChildren<Rigidbody>(true);
            Log.LogDebug($"Updating {allRigidBodies.Length} RigidBodies");
            for (int bodyNum = 0; bodyNum < allRigidBodies.Length; bodyNum++)
            {
                if (allRigidBodies[bodyNum] != _sourceRigidbody)
                {
                    allRigidBodies[bodyNum].mass = _backupMass[bodyNum];
                    allRigidBodies[bodyNum].drag = _backupDrag[bodyNum];
                }
            }
        }

        /// <summary>
        /// Set drag and mass to zero of all child Rigidbodies
        /// </summary>
        private void ResetRigidBodies()
        {
            Log.LogDebug("ResetRigidBodies");
            Rigidbody[] allRigidBodies = gameObject.GetComponentsInChildren<Rigidbody>(true);
            Log.LogDebug($"Updating {allRigidBodies.Length} RigidBodies");
            _backupMass = new float[allRigidBodies.Length];
            _backupDrag = new float[allRigidBodies.Length];
            for (int bodyNum = 0; bodyNum < allRigidBodies.Length; bodyNum++)
            {
                if (allRigidBodies[bodyNum] != _sourceRigidbody)
                {
                    _backupMass[bodyNum] = allRigidBodies[bodyNum].mass;
                    allRigidBodies[bodyNum].mass = 0.0f;
                    _backupDrag[bodyNum] = allRigidBodies[bodyNum].drag;
                    allRigidBodies[bodyNum].drag = 0.0f;
                }
            }
            UWE.Utils.SetIsKinematicAndUpdateInterpolation(_sourceRigidbody, false, false);
        }

        /// <summary>
        /// Calculate the amount of torque to apply to rotate
        /// between current and target transforms
        /// </summary>
        private Vector3 ComputeRotationTorque(Transform targetTransform)
        {
            Vector3 targetDirection = targetTransform.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

            // Rotate from our current rotation to desired rotation
            Quaternion deltaRotation = targetRotation * Quaternion.Inverse(transform.rotation);

            // Convert to angle axis representation so we can do math with angular velocity
            Vector3 angleAxis;
            deltaRotation.ToAngleAxis(out float axisMagnitude, out angleAxis);
            angleAxis.Normalize();

            // Angular velocity we need to achieve
            Vector3 angluarVelocity = angleAxis * axisMagnitude * Mathf.Deg2Rad / Time.fixedDeltaTime * 0.5f;
            angluarVelocity -= _sourceRigidbody.angularVelocity;

            // to multiply with inertia tensor local then rotationTensor coords
            Vector3 transformDirection = transform.InverseTransformDirection(angluarVelocity);
            Vector3 tensorRotation = transformDirection;
            tensorRotation = _sourceRigidbody.inertiaTensorRotation * tensorRotation;
            tensorRotation.Scale(_sourceRigidbody.inertiaTensor);
            Vector3 transformRotation = Quaternion.Inverse(_sourceRigidbody.inertiaTensorRotation) * tensorRotation;
            Vector3 torqueToApply = transform.TransformDirection(transformRotation);
            return torqueToApply;
        }
    }
}