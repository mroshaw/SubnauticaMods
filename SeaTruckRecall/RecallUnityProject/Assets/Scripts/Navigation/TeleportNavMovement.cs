using UnityEngine;
using static DaftAppleGames.SeatruckRecall_BZ.SeaTruckDockRecallPlugin;

namespace DaftAppleGames.SeatruckRecall_BZ.Navigation
{
    internal class TeleportNavMovement : WaypointNavigation
    {
        // Movement properties for this method of navigation
        protected override float RotateSpeed => 20.0f;
        protected override float MoveSpeed => 5.0f;

        /// <summary>
        /// Implement the MoveUpdate abstract method, using direct setting
        /// of position and rotation ("Teleport")
        /// </summary>
        protected override void MoveUpdate(Transform targetTransform)
        {
            Log.LogDebug($"MoveUpdate: Moving to {targetTransform.position}");
            transform.position = targetTransform.position;
        }

        /// <summary>
        /// Implement the RotateUpdate interface method, using the Rigidbody to move the Source transform
        /// </summary>
        protected override void RotateUpdate(Transform targetTransform)
        {
            // Log.LogDebug($"RotateUpdate: Rotating from {transform.rotation}. Source position: {transform.position}, looking at: {targetTransform.position}");
            transform.LookAt(targetTransform.position);
            // Log.LogDebug($"RotateUpdate: New rotation is: {transform.rotation}");
        }

        /// <summary>
        /// Apply a small force to push the teleported vehicle into the docking trigger
        /// </summary>
        protected internal override void NavComplete()
        {
            // Force docking
            Log.LogInfo("Teleport Movement: Nudging...");
            Nudge(10.0f);
            Log.LogInfo("Teleport Movement: Nudged.");
        }
    }
}