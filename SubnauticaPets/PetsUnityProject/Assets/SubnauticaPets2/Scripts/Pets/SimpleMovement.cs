using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DaftAppleGames.SubnauticaPets.Pets
{
    internal class SimpleMovement : MonoBehaviour
    {
        private const string ObstacleDetector = "ObstacleDetector";
        private const string GroundDetector = "GroundDetector";

        [Header("Movement")] public float idleCoolDown = 5.0f;
        public float moveSpeed = 0.8f;
        public float rotateSpeed = 4.0f;
        public int moveProbability = 20;
        public float minTravelDistance = 3.0f;
        public float maxTravelDistance = 10.0f;
        public float minTravelAngle = 30.0f;
        public float maxTravelAngle = 140.0f;
        public float arrivedTolerance = 0.2f;

        [Header("Ground")] public float fallSpeed = 5f;
        public float fallForwardStep = 0.2f;
        public float fallStep = 0.9f;
        public float groundCheckTolerance = 0.1f;
        public float groundedDetectionRange = 0.2f;

        [Header("Obstacles")] public float collisionCastRadius = 0.15f;
        public float obstacleDetectionRange = 0.0f;
        public float rotateToTargetTolerance = 5.0f;

        // Debugging in Unity inspector
        [Header("Debug Movement")] [SerializeField] private Transform targetMarker;
        [SerializeField] private float idleTimer;
        [SerializeField] private Vector3 moveTarget;
        [SerializeField] private float distanceToDestination;
        [SerializeField] private bool isMoving;
        [SerializeField] private bool isStopped;

        [Header("Debug Obstacles")] [SerializeField] private bool isObstacleDetected;
        [SerializeField] private GameObject lastCollisionObstacle;
        [SerializeField] private bool isRotatingFromObstacle;
        [SerializeField] private float angleToTarget;

        [Header("Debug Grounded")] [SerializeField] private float distanceToGround;
        [SerializeField] private bool isGrounded;
        [SerializeField] private bool isFalling;
        [SerializeField] private GameObject groundObstacle;
        [SerializeField] private Vector3 groundPosition;

        private Transform _obstacleDetectorOrigin;
        private Transform _middleDetectionOrigin;
        private Transform _groundDetectorOrigin;

        // Cached objects to save on garbage
        private readonly RaycastHit[] _cachedHits = new RaycastHit[10];
        private Vector3 _cachedHitPosition;
        private GameObject _cachedHitGameObject;
        private Vector3 _newVelocity = Vector3.zero;

        private CustomPetAnimator _petAnimator;
        private Rigidbody _rigidBody;

        /// <summary>
        /// Private setter for IsMoving
        /// </summary>
        private bool IsMoving
        {
            get => isMoving;
            set
            {
                isMoving = value;
                _petAnimator.SetMoving(value);
            }
        }

        private void Awake()
        {
            _rigidBody = GetComponent<Rigidbody>();
            _petAnimator = GetComponent<CustomPetAnimator>();

            // Calculate dimension and position of collision detection boxes
            ConfigureDetectors();
        }

        private void Start()
        {
            ConfigureRigidBody();

            // Start off idle
            ToIdle();
        }


        /// <summary>
        /// Determine state and action
        /// </summary>
        private void Update()
        {
            CheckIsGrounded();
            HandleFalling();
            HandleMovement();

            // Are we ready to move again?
            if (CanMove())
            {
                // Decide if we want to move
                if (MakeDecision(moveProbability))
                {
                    StartMoving();
                }
            }
        }

        /// <summary>
        /// Run in Update/FixedUpdate to move the Pet around
        /// </summary>
        private void HandleMovement()
        {
            if (isStopped)
            {
                return;
            }

            if (!IsMoving)
            {
                UpdateIdleTimer();
                return;
            }

            // Remove height from the move target
            Vector3 flatTarget = new Vector3(moveTarget.x, transform.position.y, moveTarget.z);

            // Don't move while we're turning from obstacles
            if (!isRotatingFromObstacle)
            {
                MoveToTransform(flatTarget);
                // MoveToKinematic(flatTarget);

                // See if there's anything blocking our way
                isObstacleDetected = CheckForObstacles(out _cachedHitPosition, out _cachedHitGameObject,
                    obstacleDetectionRange);
                if (isObstacleDetected && IsMoving && !isRotatingFromObstacle)
                {
                    ToIdle();
                }
            }

            // See how far we've got left to go
            distanceToDestination = Vector3.Distance(transform.position, flatTarget);

            // We've arrived
            if (distanceToDestination <= arrivedTolerance)
            {
                ToIdle();
                return;
            }

            if (isObstacleDetected && !isRotatingFromObstacle)
            {
                isRotatingFromObstacle = true;
            }

            // Rotate away from obstacle before moving again
            if (isRotatingFromObstacle)
            {
                // Calculate the direction vector from the selfObject to the targetObject
                Vector3 directionToTarget = (moveTarget - transform.position).normalized;

                // Calculate the angle between the forward direction and the direction to the target
                angleToTarget = Math.Abs(Vector3.Angle(transform.forward, directionToTarget));

                if (angleToTarget < rotateToTargetTolerance)
                {
                    isRotatingFromObstacle = false;
                }
            }

            // RotateToKinematic(flatTarget);
            RotateToTransform(flatTarget);
        }

        #region Kinematic Move Code

        private void MoveToKinematic(Vector3 targetPosition)
        {
            Vector3 newPos = Vector3.MoveTowards(_rigidBody.position, targetPosition, moveSpeed * Time.fixedDeltaTime);

            // Use Rigidbody.MovePosition for smooth, physics-safe movement
            _rigidBody.MovePosition(newPos);
        }

        private void RotateToKinematic(Vector3 targetPosition)
        {
            Vector3 direction = targetPosition - _rigidBody.position;
            if (direction.sqrMagnitude > 0.001f) // avoid NaNs when already at target
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction.normalized, Vector3.up);

                // Smoothly rotate towards target
                Quaternion newRotation = Quaternion.RotateTowards(_rigidBody.rotation, targetRotation,
                    rotateSpeed * Time.fixedDeltaTime);
                newRotation.x = 0;
                newRotation.z = 0;
                newRotation = newRotation.normalized;
                _rigidBody.MoveRotation(newRotation);
            }
        }

        private void FallKinematic()
        {
            Vector3 fallingPosition =
                new Vector3(transform.position.x, transform.position.y - fallStep, transform.position.z);
            Vector3 newPos = Vector3.MoveTowards(_rigidBody.position, fallingPosition, fallSpeed * Time.fixedDeltaTime);
            _rigidBody.MovePosition(newPos);
        }

        #endregion

        #region Transform Move Code

        private void MoveToTransform(Vector3 targetPosition)
        {
            Vector3 currentVelocity = _rigidBody.velocity;

            // Prevent SmoothDamp from dropping the character
            float currentHeight = transform.position.y;
            Vector3 newPosition =
                Vector3.SmoothDamp(transform.position, targetPosition, ref _newVelocity, 0.5f, moveSpeed);
            newPosition.y = currentHeight;
            transform.position = newPosition;
        }

        private void RotateToTransform(Vector3 targetPosition)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotateSpeed);
            }
        }

        private void FallTransform()
        {
            Vector3 fallingPosition =
                new Vector3(transform.position.x, transform.position.y - fallStep, transform.position.z);
            fallingPosition += transform.forward * fallForwardStep;
            transform.position =
                Vector3.SmoothDamp(transform.position, fallingPosition, ref _newVelocity, 0.5f, fallSpeed);
        }

        #endregion

        /// <summary>
        /// Set up the RigidBody based on the movement type
        /// </summary>
        private void ConfigureRigidBody()
        {
            _rigidBody.isKinematic = true;
            _rigidBody.useGravity = true;
            _rigidBody.collisionDetectionMode = CollisionDetectionMode.Discrete;
            _rigidBody.interpolation = RigidbodyInterpolation.Interpolate;
            _rigidBody.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX;
        }

        /// <summary>
        /// Determines the parameters to use for collision detection
        /// </summary>
        private void ConfigureDetectors()
        {
            _obstacleDetectorOrigin = transform.Find(ObstacleDetector);
            _groundDetectorOrigin = transform.Find(GroundDetector);
        }

        /// <summary>
        /// Public method to set a new target destination
        /// </summary>
        internal void SetDestination(Vector3 newMoveTarget)
        {
            moveTarget = newMoveTarget;
            IsMoving = true;
        }

        /// <summary>
        /// Are we ready to move?
        /// </summary>
        private bool CanMove()
        {
            // If we're already moving, not ready to move again
            if (isMoving || isStopped || isFalling)
            {
                return false;
            }

            // If idle, check we've idled long enough
            return (idleTimer > idleCoolDown);
        }

        /// <summary>
        /// Make a random decision based on probability
        /// </summary>
        private bool MakeDecision(int probability)
        {
            System.Random rand = new System.Random();
            int randInt = rand.Next(0, 100);
            return randInt <= probability;
        }

        /// <summary>
        /// Simulate gravity when not grounded
        /// </summary>
        private void HandleFalling()
        {
            // Grounded and not falling, do nothing
            if (isGrounded && !isFalling)
            {
                return;
            }

            // Check to see if we've landed
            if (isGrounded && isFalling)
            {
                Debug.Log($"{gameObject.name} has stopped falling");
                transform.position = new Vector3(transform.position.x, groundPosition.y, transform.position.z);
                isFalling = false;
                Resume();
            }

            // If we've started to fall
            if (!isGrounded && !isFalling)
            {
                Debug.Log($"{gameObject.name} has started falling");
                Stop();
                isFalling = true;
            }

            // While we're falling, adjust vertical position
            if (isFalling)
            {
                // FallKinematic();
                FallTransform();
            }
        }

        /// <summary>
        /// Go to the Idle state
        /// </summary>
        private void ToIdle()
        {
            idleTimer = 0.0f;
            IsMoving = false;
            _rigidBody.velocity = Vector3.zero;
            _rigidBody.angularVelocity = Vector3.zero;
        }

        /// <summary>
        /// Go to the Moving state
        /// </summary>
        private void StartMoving()
        {
            // Get target in range
            moveTarget = isObstacleDetected
                ? GetNewTargetPosition(-transform.forward)
                : GetNewTargetPosition(transform.forward);
            IsMoving = true;
        }

        /// <summary>
        /// Carry out Idle action
        /// </summary>
        private void UpdateIdleTimer()
        {
            // Reset the action timer
            idleTimer += Time.deltaTime;
        }

        private void Resume()
        {
            isStopped = false;
        }

        internal void Stop()
        {
            IsMoving = false;
            isStopped = true;
        }

        private Vector3 GetNewTargetPosition(Vector3 direction)
        {
            // Get a random distance within the defined range
            float distance = Random.Range(minTravelDistance, maxTravelDistance);

            // Get a random angle within the defined range
            float angle = Random.Range(minTravelAngle, maxTravelAngle) * Mathf.Deg2Rad;

            // Randomly decide left or right deviation
            float sign = Random.value < 0.5f ? -1f : 1f;

            // Calculate direction relative to the transform
            Vector3 lateralOffset = Mathf.Sin(angle) * sign * transform.right;
            Vector3 targetDirection = (direction + lateralOffset).normalized;

            // Compute final position
            Vector3 newTargetPosition = transform.position + targetDirection * distance;
            newTargetPosition.y = 0;
            if (targetMarker)
            {
                targetMarker.position = newTargetPosition;
            }

            return newTargetPosition;
        }

        /// <summary>
        /// Uses a Raycast to determine grounded state
        /// </summary>
        private void CheckIsGrounded()
        {
            bool isHit = RaycastColliderCheck(_groundDetectorOrigin.position, transform.up * -1, groundedDetectionRange,
                out _cachedHitPosition, out _cachedHitGameObject, true);

            groundObstacle = _cachedHitGameObject;
            groundPosition = _cachedHitPosition;
            distanceToGround = isHit ? transform.position.y - groundPosition.y : 0;

            bool isNowGrounded = isHit && distanceToGround <= groundCheckTolerance;

            if (!isGrounded && isNowGrounded)
            {
                Debug.Log($"{gameObject.name} is now grounded on {groundObstacle.name}");
                Debug.Log($"Ground position is: {groundPosition}");
            }

            if (isGrounded && !isNowGrounded)
            {
                Debug.Log($"{gameObject.name} is no longer grounded!");
            }

            isGrounded = isNowGrounded;
        }

        /// <summary>
        /// Uses a Raycast to check for obstacles in front
        /// </summary>
        private bool CheckForObstacles(out Vector3 hitPosition, out GameObject hitGameObject, float detectionRange)
        {
            // bool hitObstacle = RaycastColliderCheck(_frontDetectionOrigin.position, _frontDetectionOrigin.forward, detectionRange, out hitPosition, out hitGameObject, true);
            bool hitObstacle = SphereColliderCheck(_obstacleDetectorOrigin.position, collisionCastRadius,
                _obstacleDetectorOrigin.forward, detectionRange, out hitPosition, out hitGameObject, true);
            lastCollisionObstacle = hitGameObject;
            if (hitObstacle)
            {
                if (hitGameObject.transform.parent && hitGameObject.transform.parent.transform.parent)
                {
                    Debug.Log(
                        $"{gameObject.name} has hit {hitGameObject.name} on {hitGameObject.transform.parent.name} on {hitGameObject.transform.parent.transform.parent.name}");
                }
                else if (hitGameObject.transform.parent)
                {
                    Debug.Log(
                        $"{gameObject.name} has hit {hitGameObject.name} on {hitGameObject.transform.parent.name}");
                }
                else
                {
                    Debug.Log($"{gameObject.name} has hit {hitGameObject.name}");
                }
            }

            return hitObstacle;
        }

        private bool SphereColliderCheck(Vector3 origin, float radius, Vector3 direction, float maxDistance,
            out Vector3 hitPosition, out GameObject hitGameObject, bool debug = false)
        {
            if (debug)
            {
                Debug.DrawLine(origin, origin + direction.normalized * maxDistance, Color.yellow);
                // Draw start/end spheres (to show the volume swept out)
                DrawSphere(origin, radius, Color.cyan);
                DrawSphere(origin + direction.normalized * maxDistance, radius, Color.cyan);
            }

            int hitCount = Physics.SphereCastNonAlloc(origin, radius, direction, _cachedHits, maxDistance,
                Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore);

            // See if any hits are on anything but self
            bool isRealHit = false;
            int hitIndex = 0;

            for (int i = 0; i < hitCount; i++)
            {
                // Check if we've hit ourselves
                if (_cachedHits[i].rigidbody && _cachedHits[i].rigidbody.gameObject == gameObject)
                {
                    continue;
                }

                isRealHit = true;
                hitIndex = i;
                break;
            }

            if (isRealHit)
            {
                hitPosition = _cachedHits[hitIndex].point;
                hitGameObject = _cachedHits[hitIndex].collider.gameObject;
                return true;
            }

            hitPosition = Vector3.zero;
            hitGameObject = null;
            return false;
        }

        void DrawSphere(Vector3 pos, float r, Color c)
        {
            var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            Destroy(sphere.GetComponent<Collider>());
            var renderer = sphere.GetComponent<Renderer>();
            renderer.material.color = c;
            sphere.transform.position = pos;
            sphere.transform.localScale = Vector3.one * r * 2f;
            Destroy(sphere, Time.deltaTime); // auto-cleanup after a frame
        }

        /// <summary>
        /// Generic Raycast routine using NonAlloc
        /// </summary>
        private bool RaycastColliderCheck(Vector3 origin, Vector3 direction, float maxDistance,
            out Vector3 hitPosition, out GameObject hitGameObject, bool debug = false)
        {
            if (debug)
            {
                Debug.DrawRay(origin, direction * maxDistance, Color.red);
            }

            int hitCount = Physics.RaycastNonAlloc(origin, direction, _cachedHits, maxDistance, ~0,
                QueryTriggerInteraction.Ignore);

            // See if any hits are on anything but self
            bool isRealHit = false;
            int hitIndex = 0;

            for (int i = 0; i < hitCount; i++)
            {
                // Check if we've hit ourselves
                if (_cachedHits[i].rigidbody && _cachedHits[i].rigidbody.gameObject == gameObject)
                {
                    continue;
                }

                isRealHit = true;
                hitIndex = i;
                break;
            }

            if (isRealHit)
            {
                hitPosition = _cachedHits[hitIndex].point;
                hitGameObject = _cachedHits[hitIndex].collider.gameObject;
                return true;
            }

            hitPosition = Vector3.zero;
            hitGameObject = null;
            return false;
        }
    }
}