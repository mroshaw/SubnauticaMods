using UnityEngine;
using UnityEngine.Events;

namespace DaftAppleGames.SubnauticaPets.Pets
{
    /// <summary>
    /// Simple movement using Unity CharacterController
    /// </summary>
    internal class SimpleMovement : MonoBehaviour
    {
        [Header("Movement Settings")]
        public float moveSpeed = 0.8f;
        public float rotateSpeed = 4.0f;
        public float arrivalTolerance = 0.2f;

        [Header("Debug")]
        [Header("Debug Movement")] [SerializeField] private Transform targetMarker;
        [SerializeField] private bool isGrounded;
        [SerializeField] private Vector3 moveDirection;
        [SerializeField] private Vector3 moveTarget;
        [SerializeField] private float distanceToTarget;
        [SerializeField] private float idleTimer;
        [SerializeField] private bool isMoving;

        [SerializeField] internal UnityEvent onArrived = new UnityEvent();
        [SerializeField] internal ControllerColliderHitEvent OnHitObstacle = new ControllerColliderHitEvent();
        
        private CharacterController _charController;
        private CustomPetAnimator _petAnimator;
        
        public class ControllerColliderHitEvent : UnityEvent<ControllerColliderHit>
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
        
        private void Awake()
        {
            _charController = gameObject.GetComponent<CharacterController>();
            _petAnimator = GetComponent<CustomPetAnimator>();
        }
        
        private void Update()
        {
            if (!isMoving)
            {
                return;
            }
            
            CheckIsGrounded();
            SetMoveDirection();
            MoveToTarget();
            RotateToTarget();
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
            if (!isMoving)
            {
                return;
            }
            
            _charController.SimpleMove(moveDirection * moveSpeed);

            if (HasArrived())
            {
                isMoving = false;
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
            Debug.Log($"{gameObject.name} hit: {hit.gameObject.name}");
            OnHitObstacle?.Invoke(hit);
        }
    }
}