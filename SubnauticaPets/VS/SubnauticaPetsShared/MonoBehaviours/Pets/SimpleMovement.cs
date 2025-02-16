using UnityEngine;

namespace DaftAppleGames.SubnauticaPets.Pets
{
    // TODO Review this file and update to your own requirements, or remove it altogether if not required
    /// <summary>
    /// Template MonoBehaviour class. Use this to add new functionality and behaviours to
    /// the game.
    /// </summary>
    internal class SimpleMovement : MonoBehaviour
    {
        public float IdleCoolDown = 2.0f;
        public float MoveRadius = 5.0f;
        public float MoveSpeed = 0.5f;
        public float MoveSmoothTime = 0.5f;
        public int MoveProbability = 20;
        public float IdleTimer = 0.0f;
        public float ObstacleDetectionRange = 0.5f;
        public float ObstacleDetectionThreshold = 0.4f;
        public Transform Eyes;

        private Vector3 _moveTarget;
        private bool _isMoving = false;

        private Animator _animator;
        private Vector3 _moveAnchor;
        private Vector3 _velocity;

        /// <summary>
        /// Public setter for IsMoving
        /// </summary>
        public bool IsMoving
        {
            get => _isMoving;
            set
            {
                _isMoving = value;
                _animator.SetBool(IsMovingAnimParameter, value);
            }
        }

        private static readonly int IsMovingAnimParameter = Animator.StringToHash("IsMoving");

        // Start is called before the first frame update
        private void Start()
        {
            // Use spawned position as move anchor
            _moveAnchor = transform.position;
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
            // Perform action
            if (_isMoving)
            {
                Move();
            }
            else
            {
                Idle();
            }

            // Are we ready to move again?
            if (CanMove())
            {
                // Decide if we want to move
                if (MakeDecision(MoveProbability))
                {
                    ToMoving();
                }
            }
        }

        /// <summary>
        /// Public method to set the target destination
        /// </summary>
        /// <param name="newMoveTarget"></param>
        public void SetDestination(Vector3 newMoveTarget)
        {
            transform.LookAt(new Vector3(newMoveTarget.x, transform.position.y, newMoveTarget.z));
            _moveTarget = newMoveTarget;
        }

        /// <summary>
        /// Are we ready to move?
        /// </summary>
        /// <returns></returns>
        private bool CanMove()
        {
            // If we're already moving, not ready to move again
            if (_isMoving)
            {
                return false;
            }

            // If idle, check we've idled long enough
            return (IdleTimer > IdleCoolDown);
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
            if (!_isMoving)
            {
                return;
            }

            // Move to target
            Vector3 adjustedMoveTarget = new Vector3(_moveTarget.x, transform.position.y, _moveTarget.z);
            transform.position = Vector3.SmoothDamp(transform.position, adjustedMoveTarget, ref _velocity, MoveSmoothTime, MoveSpeed);

            // See how far we've got left to go
            float distanceToTarget = Vector3.Distance(transform.position, adjustedMoveTarget);

            // See if there's anything blocking our way
            bool isHit = Physics.Raycast(Eyes.position, Eyes.forward, out var hit, 1.0f);
            if (isHit)
            {
                float obstacleDistance = Vector3.Distance(Eyes.position, hit.point);
                if (obstacleDistance <= ObstacleDetectionThreshold)
                {
                    // LogUtils.LogDebug(LogArea.MonoPets, $"We've hit an object: {hit.collider.gameObject}");
                    ToIdle();
                }
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
            IdleTimer = 0.0f;
            IsMoving = false;
        }

        /// <summary>
        /// Go to the Moving state
        /// </summary>
        private void ToMoving()
        {
            // Get target in range
            Vector2 randomTarget = Random.insideUnitCircle * MoveRadius;
            Vector3 newTarget = new Vector3(randomTarget.x + _moveAnchor.x, transform.position.y, randomTarget.y + _moveAnchor.z);

            // LogUtils.LogDebug(LogArea.MonoPets, 
            //    $"SimpleMovement: transform is {transform.position.x}, {transform.position.y}, {transform.position.z}." +
            //    $"Moving to {newTarget.x}, {newTarget.y}, {newTarget.z}");

            _moveTarget = newTarget;

            // Rotate to target
            transform.LookAt(newTarget);

            IsMoving = true;
        }

        /// <summary>
        /// Carry out Idle action
        /// </summary>
        private void Idle()
        {
            // Reset the action timer
            IdleTimer += Time.deltaTime;
        }
    }
}
