using System;
using UnityEngine;
using static DaftAppleGames.SeatruckRecall_BZ.SeaTruckDockRecallPlugin;

namespace DaftAppleGames.SeatruckRecall_BZ.Navigation
{
    internal abstract class NavMovement : MonoBehaviour
    {
        protected internal float RotateSpeed { get; set; }
        protected internal float MoveSpeed { get; set; }
        protected internal float SlowDistance { get; set; }
        protected internal float RotateTolerance { get; set; }
        protected internal float MoveTolerance { get; set; }

        private Rigidbody _rigidbody;

        protected bool IsMoving { get; private set; }

        private Transform _currentTarget;
        private bool _isFacingTarget;

        protected virtual void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            IsMoving = false;
        }

        protected void StartNavigation(Transform targetTransform, Action preNavDelegate = null)
        {
            Log.LogDebug("Nav Movement Start");
            preNavDelegate?.Invoke();
            _currentTarget = targetTransform;
            _isFacingTarget = false;
            IsMoving = true;
        }

        protected void StopNavigation()
        {
            Log.LogDebug("Nav Movement Stop");
            IsMoving = false;
            _currentTarget = null;
        }

        // Moves the source towards the target in an Update or FixedUpdate.
        protected abstract void MoveUpdate(Transform targetTransform);
        // Rotates the source towards the target in an Update or FixedUpdate.
        protected abstract void RotateUpdate(Transform targetTransform);

        private void Update()
        {
            if (!IsMoving)
            {
                return;
            }

            if (!_isFacingTarget)
            {
                RotateUpdate(_currentTarget);
                CheckRotationStatus();
            }
            MoveUpdate(_currentTarget);
            CheckMoveStatus();
        }

        /// <summary>
        /// Nudge object forward with RigidBody, if present
        /// </summary>
        protected void Nudge(float velocity)
        {
            if (_rigidbody)
            {
                _rigidbody.velocity = gameObject.transform.forward * velocity;
            }
        }

        private void CheckRotationStatus()
        {
            if (IsRotationComplete())
            {
                _isFacingTarget = true;
            }
        }

        /// <summary>
        /// Check status
        /// </summary>
        public virtual void CheckMoveStatus()
        {
            if (!IsMoving)
            {
                return;
            }

            if (_isFacingTarget && IsMoveComplete())
            {
                IsMoving = false;
                MoveComplete();
            }
        }

        protected abstract void MoveComplete();

        /// <summary>
        /// Called when Rotation is complete
        /// </summary>
        public virtual void RotationCompleted(Transform targetTransform)
        {
            Log.LogDebug($"Rotation Completed.");
        }

        /// <summary>
        /// Determines whether the source is now facing the target
        /// </summary>
        protected virtual bool IsRotationComplete()
        {
            Vector3 dirToTarget = (_currentTarget.position - transform.position).normalized;
            float dotProduct = Vector3.Dot(dirToTarget, transform.forward);

            Log.LogDebug($"Checking Rotation: Current dot: {dotProduct}, Tolerance dot: {RotateTolerance}");

            // Return true if source is "looking" at target
            return dotProduct > RotateTolerance;
        }

        /// <summary>
        /// Determines whether the source is within range of the target
        /// </summary>
        protected virtual bool IsMoveComplete()
        {
            float distanceToCurrentTarget = Vector3.Distance(transform.position, _currentTarget.position);
            return distanceToCurrentTarget < MoveTolerance;
        }
    }
}