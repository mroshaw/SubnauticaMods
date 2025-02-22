using UnityEngine;
using Plugin = DaftAppleGames.SeatruckRecall_BZ.SeaTruckDockRecallPlugin;

namespace DaftAppleGames.SeatruckRecall_BZ.Navigation
{
    internal class TransformNavMovement : WaypointNavigation
    {
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
            // Implement Transform based rotate on Update
            Quaternion lookAtRotation = Quaternion.LookRotation(targetTransform.position - targetTransform.position);
            targetTransform.rotation = Quaternion.Slerp(transform.rotation, lookAtRotation, Time.deltaTime * RotateSpeed);
        }

        /// <summary>
        /// Apply a small force to push the teleported vehicle into the docking trigger
        /// </summary>
        protected override void WaypointNavComplete()
        {
            // Force docking
            Plugin.Log.LogInfo("Teleport Movement: Nudging...");
            Nudge(10.0f);
            Plugin.Log.LogInfo("Teleport Movement: Nudged.");
        }
    }
}