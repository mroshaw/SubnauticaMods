using System.Collections;
using UnityEngine;
using static DaftAppleGames.SeatruckRecall_BZ.SeaTruckDockRecallPlugin;

namespace DaftAppleGames.SeatruckRecall_BZ.Navigation
{
    public enum UpdateMode { Update, FixedUpdate }

    internal abstract class NavMovement : MonoBehaviour
    {
        // Used to nudge
        private Rigidbody _rigidbody;

        protected virtual float RotateThreshold => 0.0001f;
        protected virtual float MoveTolerance => 0.2f;

        private bool _isMoving;

        private Vector3 _currentTarget;
        private bool _isFacingTarget;
        private bool _rotateBeforeMoving;

        private UpdateMode _updateMode;

        protected virtual void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _isMoving = false;
            _updateMode = GetUpdateMode();
        }

        protected virtual void StartNavigation(Vector3 targetPosition, bool rotateBeforeMoving = true)
        {
            Log.LogDebug("Nav Movement Start");
            _currentTarget = targetPosition;
            _isFacingTarget = false;
            _rotateBeforeMoving = rotateBeforeMoving;
            _isMoving = true;
            Log.LogDebug($"StartNavigation: Moving to {targetPosition}");
        }

        protected virtual void StopNavigation()
        {
            Log.LogDebug("Nav Movement Stop");
            _isMoving = false;
        }

        protected abstract UpdateMode GetUpdateMode();

        // Moves the source towards the target in an Update or FixedUpdate.
        protected abstract void MoveUpdate(Vector3 targetPosition);

        // Rotates the source towards the target in an Update or FixedUpdate.
        protected abstract void RotateUpdate(Vector3 targetPosition);

        private void FixedUpdate()
        {
            if (_updateMode != UpdateMode.FixedUpdate)
            {
                return;
            }

            Move();
        }

        private void Update()
        {
            if (_updateMode != UpdateMode.Update)
            {
                return;
            }

            Move();
        }

        private void Move()
        {
            if (!_isMoving)
            {
                return;
            }

            // DEBUG every 5 seconds
            if (Time.frameCount % (60 * 5) == 0)
            {
                // Log.LogDebug($"NavMovement: IsFacingTarget={_isFacingTarget}");
                // Log.LogDebug($"NavMovement: IsMoveComplete={IsMoveComplete()}");
                // Log.LogDebug($"NavMovement: IsMoving={IsMoving}");
            }


            if (!_isFacingTarget)
            {
                RotateUpdate(_currentTarget);
                CheckRotationStatus();
            }
            if ((_rotateBeforeMoving && _isFacingTarget) || !_rotateBeforeMoving)
            {
                MoveUpdate(_currentTarget);
            }
            CheckMoveStatus();
        }

        /// <summary>
        /// Nudge object forward with RigidBody, if present
        /// </summary>
        protected void Nudge(float velocity)
        {
            if (_rigidbody)
            {
                StartCoroutine(NudgeAsync(velocity));
            }
        }

        private IEnumerator NudgeAsync(float velocity)
        {
            _rigidbody.velocity = transform.forward * velocity;
            yield return new WaitForSeconds(0.2f);
            _rigidbody.velocity = transform.forward * (-1 * velocity);
            yield return new WaitForSeconds(0.2f);
            _rigidbody.velocity = transform.forward * velocity;
        }

        private void CheckRotationStatus()
        {
            if (IsRotationComplete() || IsMoveComplete())
            {
                _isFacingTarget = true;
            }
        }

        /// <summary>
        /// Check status
        /// </summary>
        public virtual void CheckMoveStatus()
        {
            if (!_isMoving)
            {
                return;
            }

            if (_isFacingTarget && IsMoveComplete())
            {
                Log.LogDebug($"NavMovement: MoveComplete");
                _isMoving = false;
                NavMoveComplete();
            }
        }

        protected abstract void NavMoveComplete();

        /// <summary>
        /// Called when Rotation is complete
        /// </summary>
        public virtual void RotationCompleted()
        {
            Log.LogDebug($"Rotation Completed.");
        }

        /// <summary>
        /// Determines whether the source is now facing the target
        /// </summary>
        protected virtual bool IsRotationComplete()
        {
            Vector3 dirToTarget = _currentTarget - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(dirToTarget);

            // Return true if source is "looking" at target
            float rotateAngle = Quaternion.Angle(transform.rotation, targetRotation);
            if (rotateAngle < RotateThreshold)
            {
                // Make sure rotation is fully complete
                transform.rotation = targetRotation;
                Log.LogDebug($"Rotation complete");
                return true;
            }
            // Log.LogDebug($"Checking Rotation: Rotate Angle is: {rotateAngle}, Tolerance is: {RotateThreshold}");
            return false;
        }

        /// <summary>
        /// Determines whether the source is within range of the target
        /// </summary>
        protected virtual bool IsMoveComplete()
        {
            float distanceToCurrentTarget = Vector3.Distance(transform.position, _currentTarget);
            bool moveComplete = distanceToCurrentTarget < MoveTolerance;
            if (moveComplete)
            {
                Log.LogDebug($"Move complete");
                return true;
            }
            return false;
        }

        /// <summary>
        /// Aligns the transform to that of the target, such that they are 'facing'
        /// the same direction.
        /// </summary>
        protected void AlignRotation(Transform targetTransform)
        {
            Quaternion newRotation = Quaternion.LookRotation(targetTransform.forward, Vector3.up);
            transform.rotation = newRotation;
        }
    }
}