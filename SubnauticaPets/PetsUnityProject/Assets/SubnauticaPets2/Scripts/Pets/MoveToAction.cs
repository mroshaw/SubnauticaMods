using UnityEngine;

namespace DaftAppleGames.SubnauticaPets.Pets
{
    /// <summary>
    /// Action to move to given target
    /// </summary>
    internal class MoveToAction : PetAction
    {
        [Header("Action Settings")]
        public Vector3 movePosition;
        
        private SimpleMovement _simpleMovement;
        
        internal override void Init()
        {
            _simpleMovement = GetComponent<SimpleMovement>();
        }

        internal override void StartAction()
        {
            _simpleMovement.onArrived.AddListener(ArrivedAtTarget);
            _simpleMovement.OnHitObstacle.AddListener(HitObstacle);
            _simpleMovement.MoveToNewTarget(movePosition);
        }

        internal override void EndAction()
        {
            _simpleMovement.onArrived.RemoveListener(ArrivedAtTarget);
            _simpleMovement.OnHitObstacle.RemoveListener(HitObstacle);
        }
        
        private void ArrivedAtTarget()
        {
            ActionCompleted();
        }

        private void HitObstacle(Vector3 hit)
        {
            ActionCompleted();
        }
        
        internal override void UpdateAction()
        {
        }
        
    }
}