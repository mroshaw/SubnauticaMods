using UnityEngine;
using Plugin = DaftAppleGames.SeatruckRecall_BZ.SeaTruckDockRecallPlugin;

namespace DaftAppleGames.SeatruckRecall_BZ.Navigation
{
    internal class TransformNavMovement : BaseNavMovement, INavMovement
    {
        public override void Start()
        {
            base.Start();
        }

        /// <summary>
        /// Implement the MoveUpdate interface method, using the Rigidbody to move the Source transform
        /// </summary>
        public bool MoveUpdate(Transform targetTransform)
        {
            // Implement Transform based move on Update
            SourceTransform.position = Vector3.Lerp(transform.position, targetTransform.position, Time.deltaTime * MoveSpeed);

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
            // Implement Transform based rotate on Update
            Quaternion lookAtRotation = Quaternion.LookRotation(targetTransform.position - SourceTransform.position);
            SourceTransform.rotation = Quaternion.Slerp(transform.rotation, lookAtRotation, Time.deltaTime * RotateSpeed);

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
            SourceTransform.position = targetTransform.position;
        }

        /// <summary>
        /// Override the RotationCompleted to set the rotation and reset Rigidbody velocity
        /// </summary>
        /// <param name="targetTransform"></param>
        public override void RotationCompleted(Transform targetTransform)
        {
            base.RotationCompleted(targetTransform);
            SourceTransform.LookAt(targetTransform.position);
        }

        /// <summary>
        /// Apply a small force to push the teleported vehicle into the docking trigger
        /// </summary>
        public override void PostNavigate()
        {
            // Force docking
            Plugin.Log.LogInfo("Transform Movement: Nudging...");
            base.Nudge(10.0f);
            Plugin.Log.LogInfo("Transform Movement: Nudged.");
        }
    }
}
