using UnityEngine;
using Plugin = DaftAppleGames.SeatruckRecall_BZ.SeaTruckDockRecallPlugin;

namespace DaftAppleGames.SeatruckRecall_BZ.Navigation
{
    /// <summary>
    /// Base class implementing shared methods to support the INavMovement interface
    /// </summary>
    internal class BaseNavMovement : MonoBehaviour
    {
        // Public properties
        public float RotateSpeed { get; set; }
        public float MoveSpeed { get; set; }
        public float SlowDistance { get; set; }
        public float RotateTolerance { get; set; }
        public float MoveTolerance { get; set; }

        private Rigidbody _rigidbody;

        /// <summary>
        /// Nudge object forward with RigidBody, if present
        /// </summary>
        public void Nudge(float velocity)
        {
            if (_rigidbody)
            {
                _rigidbody.velocity = SourceGameObject.transform.forward * velocity;
            }
        }

        // Private fields
        public GameObject SourceGameObject;
        public Transform SourceTransform;

        public virtual void Start()
        {
            SourceGameObject = gameObject;
            SourceTransform = gameObject.transform;
            _rigidbody = GetComponent<Rigidbody>();
        }

        /// <summary>
        /// Called when Movement is complete
        /// </summary>
        /// <param name="targetTransform"></param>
        public virtual void MoveCompleted(Transform targetTransform)
        {
            Plugin.Log.LogDebug($"Move Completed.");
        }

        /// <summary>
        /// Called when Rotation is complete
        /// </summary>
        /// <param name="targetTransform"></param>
        public virtual void RotationCompleted(Transform targetTransform)
        {
            Plugin.Log.LogDebug($"Rotation Completed.");
        }

        /// <summary>
        /// Determines whether the source is now facing the target
        /// </summary>
        /// <param name="targetTransform"></param>
        /// <returns></returns>
        public virtual bool IsRotationComplete(Transform targetTransform)
        {
            Vector3 dirToTarget = (targetTransform.position - SourceTransform.position).normalized;
            float dotProduct = Vector3.Dot(dirToTarget, SourceTransform.forward);

            Plugin.Log.LogDebug($"Checking Rotation: Current dot: {dotProduct}, Tolerance dot: {RotateTolerance}");

            // Return true if source is "looking" at target
            return dotProduct > RotateTolerance;
        }

        /// <summary>
        /// Determines whether the source is within range of the target
        /// </summary>
        /// <param name="targetTransform"></param>
        /// <returns></returns>
        public virtual bool IsMoveComplete(Transform targetTransform)
        {
            float distanceToCurrentTarget = Vector3.Distance(SourceTransform.position, targetTransform.position);

            return distanceToCurrentTarget < MoveTolerance;
        }

        public virtual void PreNavigate()
        {
        }

        public virtual void PostNavigate()
        {
        }
    }
}
