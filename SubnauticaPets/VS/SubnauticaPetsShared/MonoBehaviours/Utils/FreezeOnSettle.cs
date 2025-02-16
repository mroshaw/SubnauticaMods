using System.Collections;
using UnityEngine;

namespace DaftAppleGames.SubnauticaPets.Utils
{
    public enum FreezeCheckType { Velocity, GroundCheck, Both, Either }

    /// <summary>
    /// Locks RigidBody constraints once the object has settled
    /// </summary>
    internal class FreezeOnSettle : MonoBehaviour
    {
        public float FloorOffset = 0.08f;
        public float VelocityThreshold = 0.035f;
        public float StartDelay = 1.0f;
        public float RayCastDistance = 0.5f;
        public float MaxTimeToWait = 3.0f;
        private float CheckTime = 2.0f;

        public FreezeCheckType CheckType = FreezeCheckType.Both;
        private bool _isStarted = false;

        private float _checkTimer = 0.0f;
        private float _movingTimer = 0.0f;
        private float _waitTimer = 0.0f;

        private Rigidbody _rigidbody;
        [SerializeField]
        private bool _isFrozen = false;
        [SerializeField]
        private float _distanceToBottom;
        [SerializeField]
        private float _velocityMagnitude;

        [SerializeField]
        private bool _isOnFloor = false;
        [SerializeField]
        private bool _hasStoppedMoving = false;
        [SerializeField]
        private bool _hasStartedMoving = false;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            StartCoroutine(WaitToStartAsync());
        }

        /// <summary>
        /// Unity Awake method. Runs every frame so remove this if not required.
        /// Runs frequently, so remove if not required.
        /// </summary>
        public void Update()
        {
            if (_isFrozen || !_isStarted)
            {
                return;
            }

            if (!_hasStartedMoving)
            {
                _hasStartedMoving = HasStartedMoving();
                return;
            }

            _waitTimer += Time.deltaTime;

            if (_waitTimer > MaxTimeToWait)
            {
                FreezeMovement();
            }

            // While we're moving, do nothing
            _isOnFloor = IsOnFloor();
            _hasStoppedMoving = HasStoppedMoving();

            if (CheckType == FreezeCheckType.Velocity && _hasStoppedMoving)
            {
                // Debug.Log("Velocity Threshold Reached");
                FreezeMovement();
            }

            if (CheckType == FreezeCheckType.GroundCheck && _isOnFloor)
            {
                // Debug.Log("Ground Threshold Reached");
                FreezeMovement();
            }

            if (CheckType == FreezeCheckType.Both && _hasStoppedMoving && _isOnFloor)
            {
                // Debug.Log("Both Thresholds Reached");
                FreezeMovement();
            }

            if (CheckType == FreezeCheckType.Either && (_hasStoppedMoving || _isOnFloor))
            {
                // Debug.Log($"Either Threshold Reached: HasStoppedMoving = {_hasStoppedMoving}, IsOnFloor = {_isOnFloor}");
                FreezeMovement();
            }
        }

        /// <summary>
        /// Determine if the object has started moving
        /// </summary>
        /// <returns></returns>
        private bool HasStartedMoving()
        {
            if (_rigidbody.velocity.magnitude > 0)
            {
                _movingTimer += Time.deltaTime;
            }
            else
            {
                _movingTimer = 0.0f;
            }

            return _movingTimer > CheckTime;
        }

        /// <summary>
        /// Freezes the object rigidbody
        /// </summary>
        private void FreezeMovement()
        {
            _rigidbody.isKinematic = true;
            _rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            _isFrozen = true;

            Destroy(this);
        }

        /// <summary>
        /// Wait a few seconds before starting
        /// </summary>
        /// <returns></returns>
        private IEnumerator WaitToStartAsync()
        {
            yield return new WaitForSeconds(StartDelay);
            _rigidbody.isKinematic = false;
            _rigidbody.useGravity = true;
            _isStarted = true;
        }

        /// <summary>
        /// Determines if object is on the floor
        /// </summary>
        /// <returns></returns>
        private bool IsOnFloor()
        {
            if (CheckType == FreezeCheckType.Velocity)
            {
                return false;
            }
            bool isHit = Physics.Raycast(transform.position, -Vector3.up, out var hit, RayCastDistance);
            _distanceToBottom = Vector3.Distance(transform.position, hit.point);
            return _distanceToBottom < FloorOffset && isHit;
        }

        /// <summary>
        /// Determines if object has stopped moving
        /// </summary>
        /// <returns></returns>
        private bool HasStoppedMoving()
        {
            if (CheckType == FreezeCheckType.GroundCheck)
            {
                return false;
            }
            _velocityMagnitude = _rigidbody.velocity.magnitude;

            if (_rigidbody.velocity.magnitude < VelocityThreshold)
            {
                _checkTimer += Time.deltaTime;
            }
            else
            {
                _checkTimer = 0.0f;
            }

            return _checkTimer > CheckTime;
        }
    }
}
