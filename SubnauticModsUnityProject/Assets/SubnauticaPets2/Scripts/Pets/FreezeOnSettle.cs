using System.Collections;
using UnityEngine;

namespace DaftAppleGames.SubnauticaPets.MonoBehaviours.Utils
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
        public FreezeCheckType CheckType = FreezeCheckType.Both;
        private bool _isStarted = false;

        private Rigidbody _rigidbody;
        [SerializeField]
        private bool _isFrozen = false;
        [SerializeField]
        private float _distanceToBottom;

        [SerializeField]
        private bool _isOnFloor = false;
        [SerializeField]
        private bool _hasStoppedMoving = false;

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

            // While we're moving, do nothing
            _isOnFloor = IsOnFloor();
            _hasStoppedMoving = HasStoppedMoving();

            if (CheckType == FreezeCheckType.Velocity && _hasStoppedMoving)
            {
                Debug.Log("Velocity Threshold Reached");
                FreezeMovement();
            }

            if (CheckType == FreezeCheckType.GroundCheck && _isOnFloor)
            {
                Debug.Log("Ground Threshold Reached");
                FreezeMovement();
            }

            if (CheckType == FreezeCheckType.Both && _hasStoppedMoving && _isOnFloor)
            {
                Debug.Log("Both Thresholds Reached");
                FreezeMovement();
            }

            if (CheckType == FreezeCheckType.Either && (_hasStoppedMoving || _isOnFloor))
            {
                Debug.Log($"Either Threshold Reached: HasStoppedMoving = {_hasStoppedMoving}, IsOnFloor = {_isOnFloor}");
                FreezeMovement();
            }
        }

        /// <summary>
        /// Freezes the object rigidbody
        /// </summary>
        private void FreezeMovement()
        {
            _rigidbody.isKinematic = true;
            _isFrozen = true;
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
            return _rigidbody.velocity.magnitude < VelocityThreshold;
        }
    }
}
