using UnityEngine;
using Random = UnityEngine.Random;

namespace DaftAppleGames.SubnauticaPets.Pets
{
    /// <summary>
    /// Simple movement using Unity CharacterController
    /// </summary>
    internal class WanderAction : PetAction
    {
        [Header("Action Settings")]
        public float minTravelDistance = 2.0f;
        public float maxTravelDistance = 10.0f;
        public float minTravelAngle = 30.0f;
        public float maxTravelAngle = 140.0f;
        
        private SimpleMovement _simpleMovement;
        
        internal override void Init()
        {
            _simpleMovement = GetComponent<SimpleMovement>();
        }

        internal override void StartAction()
        {
            // Pick a random target and move
            Vector3 newTarget = GetNewTargetPosition(transform.forward);
            _simpleMovement.onArrived.AddListener(ArrivedAtTarget);
            _simpleMovement.OnHitObstacle.AddListener(HitObstacle);
            _simpleMovement.MoveToNewTarget(newTarget);
        }

        internal override void EndAction()
        {
            _simpleMovement.onArrived.RemoveListener(ArrivedAtTarget);
            _simpleMovement.OnHitObstacle.RemoveListener(HitObstacle);
            _simpleMovement.Stop();
        }
        
        private void ArrivedAtTarget()
        {
            ActionCompleted();
        }

        private void HitObstacle(Vector3 direction)
        {
            Vector3 newTarget = GetNewTargetPosition(direction);
            _simpleMovement.MoveToNewTarget(newTarget);
        }
        
        internal override void UpdateAction()
        {
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

            return newTargetPosition;
        }
    }
}