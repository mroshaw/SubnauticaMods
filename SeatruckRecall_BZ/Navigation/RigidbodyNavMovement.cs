using UnityEngine;
using Plugin = DaftAppleGames.SeatruckRecall_BZ.SeaTruckDockRecallPlugin;

namespace DaftAppleGames.SeatruckRecall_BZ.Navigation
{
    internal class RigidbodyNavMovement : BaseNavMovement, INavMovement
    {
        // Private fields
        private Vector3 _currentDirection;
        private float _distanceToTarget;
        private Rigidbody _sourceRigidbody;

        // Backup of child rigidbody values
        private float[] backupMass;
        private float[] backupDrag;

        /// <summary>
        /// Initialise the component
        /// </summary>
        public override void Start()
        {
            base.Start();
            _sourceRigidbody = SourceGameObject.gameObject.GetComponentInChildren<Rigidbody>();
            _sourceRigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        }

        /// <summary>
        /// Implement the MoveUpdate interface method, using the Rigidbody to move the Source transform
        /// </summary>
        public bool MoveUpdate(Transform targetTransform)
        {
            _distanceToTarget = Vector3.Distance(SourceTransform.position, targetTransform.position);
            _currentDirection = (targetTransform.position - SourceTransform.position).normalized;
            if (_distanceToTarget > SlowDistance)
            {
                _sourceRigidbody.velocity = _currentDirection * MoveSpeed;
            }

            if (IsMoveComplete(targetTransform))
            {
                MoveCompleted(targetTransform);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Implement the RotateUpdate interface method, using the Rigidbody to move the Source transform
        /// </summary>
        public bool RotateUpdate(Transform targetTransform)
        {
            Vector3 torque = ComputeRotationTorque(targetTransform);
            _sourceRigidbody.AddTorque(torque, ForceMode.Impulse);
            if (IsRotationComplete(targetTransform))
            {
                RotationCompleted(targetTransform);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Override the MoveCompleted to set the position and reset Rigidbody velocity
        /// </summary>
        /// <param name="targetTransform"></param>
        public override void MoveCompleted(Transform targetTransform)
        {
            base.MoveCompleted(targetTransform);
            _sourceRigidbody.angularVelocity = Vector3.zero;
            _sourceRigidbody.velocity = Vector3.zero;
            SourceTransform.position = targetTransform.position;
        }

        /// <summary>
        /// Override the RotationCompleted to set the rotation and reset Rigidbody velocity
        /// </summary>
        /// <param name="targetTransform"></param>
        public override void RotationCompleted(Transform targetTransform)
        {
            base.RotationCompleted(targetTransform);
            _sourceRigidbody.angularVelocity = Vector3.zero;
            _sourceRigidbody.velocity = Vector3.zero;
            SourceTransform.LookAt(targetTransform.position);
        }

        /// <summary>
        /// Set drag and mass to zero of all child Rigidbodies
        /// </summary>
        public override void PreNavigate()
        {
            Plugin.Log.LogDebug("RigidBodyNavMovement PreNavigate");
            Rigidbody[] allRigidBodies = SourceTransform.gameObject.GetComponentsInChildren<Rigidbody>(true);
            Plugin.Log.LogDebug($"Updating {allRigidBodies.Length} RigidBodies");
            backupMass = new float[allRigidBodies.Length];
            backupDrag = new float[allRigidBodies.Length];
            for (int bodyNum = 0; bodyNum < allRigidBodies.Length; bodyNum++)
            {
                if (allRigidBodies[bodyNum] != _sourceRigidbody)
                {
                    backupMass[bodyNum] = allRigidBodies[bodyNum].mass;
                    allRigidBodies[bodyNum].mass = 0.0f;
                    backupDrag[bodyNum] = allRigidBodies[bodyNum].drag;
                    allRigidBodies[bodyNum].drag = 0.0f;
                }
            }

            UWE.Utils.SetIsKinematicAndUpdateInterpolation(_sourceRigidbody, false, false);
        }

        /// <summary>
        /// Reset drag and mass on child Rigidbodies
        /// </summary>
        public override void PostNavigate()
        {
            Plugin.Log.LogDebug("RigidBodyNavMovement PostNavigate");
            Rigidbody[] allRigidBodies = SourceTransform.gameObject.GetComponentsInChildren<Rigidbody>(true);
            Plugin.Log.LogDebug($"Updating {allRigidBodies.Length} RigidBodies");
            for (int bodyNum = 0; bodyNum < allRigidBodies.Length; bodyNum++)
            {
                if (allRigidBodies[bodyNum] != _sourceRigidbody)
                {
                    allRigidBodies[bodyNum].mass = backupMass[bodyNum];
                    allRigidBodies[bodyNum].drag = backupDrag[bodyNum];
                }
            }
        }

        /// <summary>
        /// Calculate the amount of torque to apply to rotate
        /// between current and target transforms
        /// </summary>
        /// <param name="targetTransform"></param>
        /// <returns></returns>
        private Vector3 ComputeRotationTorque(Transform targetTransform)
        {
            Vector3 targetDirection = targetTransform.position - SourceTransform.position;
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

            // Rotate from our current rotation to desired rotation
            Quaternion deltaRotation = targetRotation * Quaternion.Inverse(SourceTransform.rotation);

            // Convert to angle axis representation so we can do math with angular velocity
            Vector3 angleAxis;
            deltaRotation.ToAngleAxis(out float axisMagnitude, out angleAxis);
            angleAxis.Normalize();

            // Angular velocity we need to achieve
            Vector3 angluarVelocity = angleAxis * axisMagnitude * Mathf.Deg2Rad / Time.fixedDeltaTime * 0.5f;
            angluarVelocity -= _sourceRigidbody.angularVelocity;

            // to multiply with inertia tensor local then rotationTensor coords
            Vector3 transformDirection = SourceTransform.InverseTransformDirection(angluarVelocity);
            Vector3 tensorRotation = transformDirection;
            tensorRotation = _sourceRigidbody.inertiaTensorRotation * tensorRotation;
            tensorRotation.Scale(_sourceRigidbody.inertiaTensor);
            Vector3 transformRotation = Quaternion.Inverse(_sourceRigidbody.inertiaTensorRotation) * tensorRotation;
            Vector3 torqueToApply = SourceTransform.TransformDirection(transformRotation);
            return torqueToApply;
        }
    }
}
