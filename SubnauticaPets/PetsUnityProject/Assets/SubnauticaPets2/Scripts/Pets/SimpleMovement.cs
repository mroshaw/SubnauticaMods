using UnityEngine;
using UnityEngine.Events;

namespace DaftAppleGames.SubnauticaPets.Pets
{
    /// <summary>
    /// Simple movement using Unity CharacterController
    /// </summary>
    internal class SimpleMovement : MonoBehaviour
    {
        private const string MoonPoolDetectorName = "MoonPoolDetector";
        
        [Header("Movement Settings")]
        public float moveSpeed = 0.8f;
        public float rotateSpeed = 4.0f;
        public float arrivalTolerance = 0.05f;

        [Header("Debug")]
        [Header("Debug Movement")] [SerializeField] private Transform targetMarker;
        [SerializeField] private bool isGrounded;
        [SerializeField] private Vector3 moveDirection;
        [SerializeField] private Vector3 moveTarget;
        [SerializeField] private float distanceToTarget;
        [SerializeField] private bool isMoving;
        [SerializeField] private bool moonPoolDetected = false;
        [SerializeField] private bool facingMoonPool = false;
        
        [SerializeField] internal UnityEvent onArrived = new UnityEvent();
        [SerializeField] internal ControllerColliderHitEvent OnHitObstacle = new ControllerColliderHitEvent();
        
        private CharacterController _charController;
        private PetAnimator _petAnimator;
        private Rigidbody _rigidbody;
        
        public class ControllerColliderHitEvent : UnityEvent<Vector3>
        {
        }
        
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

        private void OnEnable()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.isKinematic = true;
            _rigidbody.useGravity = false;
        }
        
        private void Awake()
        {
            _charController = gameObject.GetComponent<CharacterController>();
            _petAnimator = GetComponent<PetAnimator>();
            ConfigureMoonPoolDetector();
        }
        
        private void Update()
        {
            // Ensure Rigidbody is always Kinematic
            _rigidbody.isKinematic = true;
            
            if (!IsMoving)
            {
                return;
            }
            
            CheckIsGrounded();
            SetMoveDirection();
            MoveToTarget();
            RotateToTarget();
        }

        private void ConfigureMoonPoolDetector()
        {
            Transform detectorTransform = transform.Find(MoonPoolDetectorName);
            if (!detectorTransform)
            {
                LogUtils.LogError(LogArea.MonoPets, $"No Moon Pool detector found on {gameObject.name}");
                return;
            }

            MoonPoolDetector moonPoolDetector = detectorTransform.gameObject.AddComponent<MoonPoolDetector>();
            moonPoolDetector.OnMoonPoolDetected.AddListener(MoonPoolDetected);
            moonPoolDetector.OnMoonPoolLost.AddListener(MoonPoolAvoided);
        }

        private void MoonPoolDetected()
        {
            LogUtils.LogDebug(LogArea.MonoPets, $"Moon Pool detected by {gameObject.name}");
            OnHitObstacle?.Invoke(transform.forward * -1);
            moonPoolDetected = true;
        }

        private void MoonPoolAvoided()
        {
            LogUtils.LogDebug(LogArea.MonoPets, $"Moon Pool avoided by {gameObject.name}");
            moonPoolDetected = false;
        }
        
        internal void MoveToNewTarget(Vector3 target)
        {
            moveTarget = target;
            IsMoving = true;
            
            if (targetMarker)
            {
                targetMarker.position = target;
            }
        }

        internal void Stop()
        {
            IsMoving = false;
        }
        
        /// <summary>
        /// Set the direction to the target
        /// </summary>
        private void SetMoveDirection()
        {
            moveTarget.y = transform.position.y;
            moveDirection = (moveTarget - transform.position).normalized;
            moveDirection.y = 0;
        }
        
        /// <summary>
        /// Move towards the target using the CharacterController
        /// </summary>
        private void MoveToTarget()
        {
            _charController.SimpleMove(moveDirection * moveSpeed);

            if (HasArrived())
            {
                IsMoving = false;
                onArrived?.Invoke();
            }
        }

        private void RotateToTarget()
        {
            // Rotate smoothly towards the target
            if (moveDirection != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotateSpeed * Time.deltaTime);
            }
        }
        
        private void CheckIsGrounded()
        {
            isGrounded = _charController.isGrounded;
        }

        private bool HasArrived()
        {
            distanceToTarget = Vector3.Distance(transform.position, moveTarget);
            return distanceToTarget < arrivalTolerance;
        }
        
        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            // Check for ground hit
            // If the normal points up, it's probably ground
            if (Vector3.Angle(hit.normal, Vector3.up) < 45f)
            {
                // Ground contact — ignore
                return;
            }
            // LogUtils.LogDebug(LogArea.MonoPets, $"{gameObject.name} hit: {hit.gameObject.name}");
            OnHitObstacle?.Invoke(hit.normal);
        }
    }
}