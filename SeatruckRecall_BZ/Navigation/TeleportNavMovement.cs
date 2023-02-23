using UnityEngine;

namespace DaftAppleGames.SeatruckRecall_BZ.Navigation
{
    internal class TeleportNavMovement : BaseNavMovement, INavMovement
    {
        /// <summary>
        /// Implement the MoveUpdate interface method, using direct setting
        /// of position and rotation ("Teleport")
        /// </summary>
        public bool MoveUpdate(Transform targetTransform)
        {
            SourceTransform.position = targetTransform.position;
            MoveCompleted(targetTransform);
            return true;
        }

        /// <summary>
        /// Implement the RotateUpdate interface method, using the Rigidbody to move the Source transform
        /// </summary>
        public bool RotateUpdate(Transform targetTransform)
        {
            SourceTransform.LookAt(targetTransform.position);
            RotationCompleted(targetTransform);
            return true;
        }

        /// <summary>
        /// Override the MoveCompleted to set the position and reset Rigidbody velocity
        /// </summary>
        /// <param name="targetTransform"></param>
        public override void MoveCompleted(Transform targetTransform)
        {
            base.MoveCompleted(targetTransform);
        }

        /// <summary>
        /// Override the RotationCompleted to set the rotation and reset Rigidbody velocity
        /// </summary>
        /// <param name="targetTransform"></param>
        public override void RotationCompleted(Transform targetTransform)
        {
            base.RotationCompleted(targetTransform);
        }
    }
}
