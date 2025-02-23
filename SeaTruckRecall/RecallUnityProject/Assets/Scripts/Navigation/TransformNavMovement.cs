using UnityEngine;

namespace DaftAppleGames.SeatruckRecall_BZ.Navigation
{
    internal class TransformNavMovement : WaypointNavigation
    {

        // Movement properties for this method of navigation
        protected override float RotateSpeed => 3.0f;
        protected override float MoveSpeed => 0.8f;

        /// <summary>
        /// Implement the MoveUpdate interface method, using the Rigidbody to move the Source transform
        /// </summary>
        protected override void MoveUpdate(Transform targetTransform)
        {
            // Implement Transform based move on Update
            transform.position = Vector3.Lerp(transform.position, targetTransform.position, Time.deltaTime * MoveSpeed);
        }

        /// <summary>
        /// Implement the RotateUpdate interface method, using the Rigidbody to move the Source transform
        /// </summary>
        protected override void RotateUpdate(Transform targetTransform)
        {
            Vector3 dirToTarget = targetTransform.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(dirToTarget);

            // Implement Transform based rotate on Update
            if (dirToTarget.sqrMagnitude < Mathf.Epsilon) // Check the positions aren't the same
            {
                // If they are, set the rotation without Lerp
                Log.LogDebug("Target Position is same as Source Transform. Setting rotation manually.");
                transform.LookAt(targetTransform);
                return;
            }
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * RotateSpeed);
        }

        /// <summary>
        /// Disable RigidBody on all SeaTrucks
        /// </summary>
        protected internal override void NavStarted()
        {
            Log.LogDebug($"Setting SeaTruck Rigidbody to IsKinematic: {gameObject.name}");
            UWE.Utils.SetIsKinematicAndUpdateInterpolation(gameObject, true, false);
        }

        /// <summary>
        /// Re-enable rigid bodies on all Seatrucks
        /// Apply a small force to push the teleported vehicle into the docking trigger
        /// </summary>
        protected internal override void NavComplete()
        {
            Log.LogDebug("Resetting SeaTruck Rigidbodies");

            // Reset rigidbodies
            UWE.Utils.SetIsKinematicAndUpdateInterpolation(gameObject, false, true);
            // Force docking
            Log.LogInfo("Teleport Movement: Nudging...");
            Nudge(10.0f);
            Log.LogInfo("Teleport Movement: Nudged.");
        }
    }
}