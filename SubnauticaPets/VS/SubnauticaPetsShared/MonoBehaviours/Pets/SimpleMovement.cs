using DaftAppleGames.SubnauticaPets.Utils;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DaftAppleGames.SubnauticaPets.Pets
{
    internal class SimpleMovement : MonoBehaviour
    {
        public float idleCoolDown = 2.0f;
        public float moveRadius = 5.0f;
        public float moveSpeed = 0.5f;
        public float dropSpeed = 1.5f;
        public float rotateSpeed = 5.0f;
        public float moveSmoothTime = 0.5f;
        public int moveProbabilty = 20;
        public float idleTimer = 0.0f;
        public float obstacleDetectionRange = 0.3f;
        public float groundDetectionRange = 2.0f;
        public float groundDetectionOffset = 0.4f;

        public Transform Eyes { get; set; }

        public float minTravelDistance = 2.0f;
        public float maxTravelDistance = 5.0f;
        public float minTravelAngle = 30.0f;
        public float maxTravelAngle = 150.0f;

        [SerializeField] private Vector3 moveTarget;
        [SerializeField] private bool isMoving = false;
        [SerializeField] private bool isObstacleDetected = false;
        [SerializeField] private bool isRotatingFromObstacle = false;
        [SerializeField] private bool isFalling = false;
        [SerializeField] private bool isStopped = false;
        private Animator _animator;
        private Vector3 _velocity;

        [SerializeField] private float groundHeight;

        // Cached objects to save on garbage
        private RaycastHit[] _cachedHits = new RaycastHit[10];
        private Ray _cachedRay = new Ray();
        private Vector3 _cachedHitPosition;
        private GameObject _cachedHitGameObject;

        /// <summary>
        /// Public setter for IsMoving
        /// </summary>
        public bool IsMoving
        {
            get => isMoving;
            set
            {
                isMoving = value;
                _animator.SetBool(IsMovingAnimParameter, value);
            }
        }

        private static readonly int IsMovingAnimParameter = Animator.StringToHash("IsMoving");

        // Start is called before the first frame update
        private void Start()
        {
            _animator = GetComponent<Animator>();

            // Set the eyes, if not set already
            if (!Eyes)
            {
                Eyes = transform.Find("Eyes").transform;
            }

            // Start off idle
            ToIdle();
        }

        /// <summary>
        /// Determine state and action
        /// </summary>
        private void Update()
        {

            if (isStopped)
            {
                return;
            }

            // Perform action
            if (isMoving)
            {
                Move();
            }
            else
            {
                Idle();
            }

            HandleFalling();

            // Are we ready to move again?
            if (CanMove())
            {
                // Decide if we want to move
                if (MakeDecision(moveProbabilty))
                {
                    ToMoving();
                }
            }
        }

        /// <summary>
        /// Public method to set the target destination
        /// </summary>
        /// <param name="newMoveTarget"></param>
        internal void SetDestination(Vector3 newMoveTarget)
        {
            moveTarget = newMoveTarget;
            IsMoving = true;
        }

        /// <summary>
        /// Are we ready to move?
        /// </summary>
        /// <returns></returns>
        private bool CanMove()
        {
            // If we're already moving, not ready to move again
            if (isMoving)
            {
                return false;
            }

            // If idle, check we've idled long enough
            return (idleTimer > idleCoolDown);
        }

        /// <summary>
        /// Make a random decision based on probability
        /// </summary>
        /// <returns></returns>
        private bool MakeDecision(int probability)
        {
            System.Random rand = new System.Random();
            int randInt = rand.Next(0, 100);
            return randInt <= probability;
        }

        /// <summary>
        /// Carry out Move action
        /// </summary>
        private void Move()
        {
            if (!isMoving)
            {
                return;
            }

            Vector3 adjustedMoveTarget = new Vector3(moveTarget.x, transform.position.y, moveTarget.z);

            // Rotate away from obstacle before moving again
            if (isObstacleDetected && !isRotatingFromObstacle)
            {
                isRotatingFromObstacle = true;
            }

            if (!isRotatingFromObstacle)
            {
                transform.position = Vector3.SmoothDamp(transform.position, adjustedMoveTarget, ref _velocity, moveSmoothTime, moveSpeed);
            }

            // Rotate to target
            Vector3 direction = (adjustedMoveTarget - transform.position).normalized;
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotateSpeed);
            }

            // See how far we've got left to go
            float distanceToTarget = Vector3.Distance(transform.position, adjustedMoveTarget);

            // See if there's anything blocking our way
            isObstacleDetected = CheckForObstacles(out _cachedHitPosition, out _cachedHitGameObject);
            if (isObstacleDetected && !isRotatingFromObstacle)
            {
                LogUtils.LogDebug(LogArea.MonoPets, $"We've hit an object: {_cachedHitGameObject.name}");
                ToIdle();
            }
            else
            {
                isRotatingFromObstacle = false;
            }

            // We've arrived
            if (distanceToTarget <= 0.01f)
            {
                ToIdle();
            }
        }

        /// <summary>
        /// Go to the Idle state
        /// </summary>
        private void ToIdle()
        {
            idleTimer = 0.0f;
            IsMoving = false;
        }

        internal void Stop()
        {
            isStopped = true;
        }

        internal void Restart()
        {
            isStopped = false;
        }

        /// <summary>
        /// Go to the Moving state
        /// </summary>
        private void ToMoving()
        {
            // Get target in range
            if (isObstacleDetected)
            {
                moveTarget = GetNewTargetPosition(-transform.forward);
            }
            else
            {
                moveTarget = GetNewTargetPosition(transform.forward);
            }

            IsMoving = true;

        }

        /// <summary>
        /// Carry out Idle action
        /// </summary>
        private void Idle()
        {
            // Reset the action timer
            idleTimer += Time.deltaTime;
        }


        public Vector3 GetNewTargetPosition(Vector3 direction)
        {
            // Get a random distance within the defined range
            float distance = Random.Range(minTravelDistance, maxTravelDistance);

            // Get a random angle within the defined range
            float angle = Random.Range(minTravelAngle, maxTravelAngle) * Mathf.Deg2Rad;

            // Randomly decide left or right deviation from directly behind
            float sign = Random.value < 0.5f ? -1f : 1f;

            // Calculate direction relative to the transform
            Vector3 lateralOffset = Mathf.Sin(angle) * sign * transform.right;
            Vector3 targetDirection = (direction + lateralOffset).normalized;

            // Compute final position
            Vector3 newTargetPosition = transform.position + targetDirection * distance;
            return newTargetPosition;
        }

        private bool CheckForObstacles(out Vector3 hitPosition, out GameObject hitGameObject)
        {
            bool hitObstacle = RaycastColliderCheck(Eyes.transform.position, Eyes.transform.forward, obstacleDetectionRange, out hitPosition, out hitGameObject);
            return hitObstacle;
        }

        private void CheckForGround()
        {
            Vector3 rayOrigin = transform.position + (transform.forward * -1 * groundDetectionOffset);

            if (RaycastColliderCheck(Eyes.transform.position + (Eyes.transform.forward * -1 * groundDetectionOffset), transform.up * -1, groundDetectionRange, out _cachedHitPosition, out _cachedHitGameObject))
            {
                // LogUtils.LogDebug(LogArea.MonoPets, $"Ground detected: {_cachedHitGameObject}");
                groundHeight = _cachedHitPosition.y;
            }
        }

        private void HandleFalling()
        {
            CheckForGround();
            isFalling = (Math.Abs(transform.position.y - groundHeight) >= 0.00001);
            // _isFalling = _groundHeight < transform.position.y;

            if (isFalling)
            {
                Vector3 adjustedMoveTarget = new Vector3(transform.position.x, groundHeight, transform.position.z);
                transform.position = Vector3.SmoothDamp(transform.position, adjustedMoveTarget, ref _velocity, moveSmoothTime, dropSpeed);
            }
        }

        private bool RaycastColliderCheck(Vector3 origin, Vector3 direction, float maxDistance, out Vector3 hitPosition, out GameObject hitGameObject)
        {
            _cachedRay.direction = direction;
            _cachedRay.origin = origin;

            Debug.DrawLine(origin, origin + direction * maxDistance);

            int numHits = Physics.RaycastNonAlloc(_cachedRay, _cachedHits, maxDistance);

            if (numHits > 0)
            {
                int closestHitIndex = 0;
                float closestHitDistance = float.MaxValue;

                for (int curHit = 0; curHit < numHits; curHit++)
                {
                    // Check we haven't hit ourselves
                    if ((!_cachedHits[curHit].collider.transform.parent) || (_cachedHits[curHit].collider.transform.parent && _cachedHits[curHit].collider.transform.parent.gameObject != gameObject))
                    {
                        float hitDistance = Vector3.Distance(transform.position, _cachedHits[curHit].point);
                        if (hitDistance < closestHitDistance)
                        {
                            closestHitDistance = hitDistance;
                            closestHitIndex = curHit;
                        }
                    }
                }
                hitPosition = _cachedHits[closestHitIndex].point;
                hitGameObject = _cachedHits[closestHitIndex].collider.gameObject;
                return true;
            }

            hitPosition = Vector3.zero;
            hitGameObject = null;
            return false;
        }
    }
}