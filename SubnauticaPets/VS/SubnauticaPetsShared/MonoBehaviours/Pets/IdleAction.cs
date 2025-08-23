using UnityEngine;
using Random = UnityEngine.Random;

namespace DaftAppleGames.SubnauticaPets.Pets
{
    /// <summary>
    /// Idle action
    /// </summary>
    internal class IdleAction : PetAction
    {
        [Header("Action Settings")]
        public float minIdleTime = 3.0f;
        public float maxIdleTime = 10.0f;
        public float chanceToPlayAnim = 10.0f;
        
        [Header("Debug")]
        [SerializeField] private float idleCounter;
        
        private SimpleMovement _simpleMovement;
        private PetAnimator _petAnimator;
        
        internal override void Init()
        {
            _simpleMovement = GetComponent<SimpleMovement>();
            _petAnimator = GetComponent<PetAnimator>();
        }

        internal override void StartAction()
        {
            idleCounter = Random.Range(minIdleTime, maxIdleTime);

            // If random chance, play action anim
            if (GetRandomBool(chanceToPlayAnim))
            {
                _petAnimator.PlayRandomBodyAnim(true);
            }
        }

        internal override void EndAction()
        {
        }
        
        internal override void UpdateAction()
        {
            idleCounter -= Time.deltaTime;

            if (idleCounter < 0)
            {
                ActionCompleted();
            }
        }
    }
}