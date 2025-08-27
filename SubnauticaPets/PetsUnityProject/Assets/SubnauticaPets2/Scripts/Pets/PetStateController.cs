using UnityEngine;

namespace DaftAppleGames.SubnauticaPets.Pets
{
    public enum PetState { Idle, Wandering, MovingTo, Dead }
    
    /// <summary>
    /// Simple State Controller for pet actions
    /// </summary>
    internal class PetStateController : MonoBehaviour
    {

        [SerializeField] private PetState currState;
        private PetAction _currentAction;
        
        private WanderAction _wanderAction;
        private IdleAction _idleAction;
        private MoveToAction _moveToAction;
        private KilledAction _killedAction;
        
        private void Awake()
        {
            _wanderAction = GetComponent<WanderAction>();
            _idleAction = GetComponent<IdleAction>();
            _moveToAction = GetComponent<MoveToAction>();
            _killedAction = GetComponent<KilledAction>();
        }
        
        private void Start()
        {
            _idleAction.Init();
            _wanderAction.Init();
            _moveToAction.Init();
            _killedAction.Init();
            
            _wanderAction.OnActionCompleted.AddListener(WanderActionComplete);
            _idleAction.OnActionCompleted.AddListener(IdleActionComplete);
            _moveToAction.OnActionCompleted.AddListener(MoveToActionComplete);
            _killedAction.OnActionCompleted.AddListener(KilledActionComplete);
            
            SetNewState(PetState.Idle);
        }

        internal void SetNewState(PetState newState)
        {
            if (currState == PetState.Dead)
            {
                return;
            }
            
            LogUtils.LogDebug(LogArea.MonoPets, $"{gameObject.name}: Changing state from: {currState} to {newState}");
            currState = newState;
            
            switch (newState)
            {
                case PetState.Idle:
                    SetNewAction(_idleAction);
                    break;
                case PetState.Wandering:
                    SetNewAction(_wanderAction);
                    break;
                case PetState.MovingTo:
                    SetNewAction(_moveToAction);
                    break;
                case PetState.Dead:
                    SetNewAction(_killedAction);
                    break;
            }
        }

        /// <summary>
        /// Called by Pet to move to player
        /// </summary>
        internal void MoveToPosition(Vector3 position)
        {
            _moveToAction.movePosition = position;
            SetNewState(PetState.MovingTo);
        }

        internal void Kill()
        {
            SetNewState(PetState.Dead);
        }
        
        private void WanderActionComplete()
        {
            SetNewState(PetState.Idle);
        }

        private void IdleActionComplete()
        {
            SetNewState(PetState.Wandering);
        }

        private void MoveToActionComplete()
        {
            SetNewState(PetState.Idle);
        }

        private void KilledActionComplete()
        {
            _currentAction = null;
        }
        
        private void SetNewAction(PetAction newAction)
        {
            _currentAction?.EndAction();
            _currentAction = newAction;
            _currentAction.StartAction();
        }
        
        private void Update()
        {
            if (!_currentAction)
            {
                return;
            }
            _currentAction.UpdateAction();
        }
    }
}