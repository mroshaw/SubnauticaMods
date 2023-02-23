using UnityEngine;

namespace DaftAppleGames.SeatruckRecall_BZ.Navigation
{
    /// <summary>
    /// Movement Interface
    /// Used to Move and Rotate a "Source" transform towards a "Target" transform
    /// </summary>
    internal interface INavMovement
    {
        float RotateSpeed { get; set; }
        float MoveSpeed { get; set; }
        float SlowDistance { get; set; }
        float RotateTolerance { get; set; }
        float MoveTolerance { get; set; }

        // Moves the source towards the target in an Update or FixedUpdate.
        bool MoveUpdate(Transform targetTransform);

        // Rotates the source towards the target in an Update or FixedUpdate.
        bool RotateUpdate(Transform targetTransform);

        void MoveCompleted(Transform targetTransform);

        void RotationCompleted(Transform targetTransform);

        void PreNavigate();

        void PostNavigate();
    }
}
