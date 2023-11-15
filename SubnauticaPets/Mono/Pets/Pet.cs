using System.Collections.Generic;
using DaftAppleGames.SubnauticaPets.Utils;
using UnityEngine;
using UnityEngine.AI;
using static DaftAppleGames.SubnauticaPets.SubnauticaPetsPlugin;

namespace DaftAppleGames.SubnauticaPets.Mono.Pets
{

    /// <summary>
    /// MonoBehaviour component providing Pet behaviours and properties.
    /// Acts as a base to be inherited by Creature specific classes
    /// </summary>
    public abstract class Pet : MonoBehaviour, IPet
    {
        // Public properties

        // Scale factor is set as the LocalScale of spawned gameobject
        // across all axis
        public abstract Vector3 ScaleFactor { get; }

        public GameObject ParentBaseGameObject { get; set; }

        // Private pet info
        private PetCreatureType _petCreatureType;
        private string _petName;
        private MoveOnSurface _moveOnSurface;
        private MoveOnGround _moveOnGround;
        private SimpleMovement _simpleMovement;
        private Animator _animator;

        private bool _canMove = false;

        // Used to keep tabs on saved pets
        private PetSaver.PetDetails _petSaverDetails;

        // private bool _isFollowingPlayer;

        /// <summary>
        /// Public getter and setter for PetCreatureType
        /// </summary>
        public PetCreatureType PetCreatureType
        {
            set => _petCreatureType = value;
            get => _petCreatureType;
        }

        /// <summary>
        /// Gets a "display friendly" version of the Pet Type
        /// </summary>
        public string PetTypeString
        {
            get => ModUtils.AddSpacesInCamelCase(_petCreatureType.ToString());
        }

        /// <summary>
        /// Gets a "display friendly" version of the Pet Name
        /// </summary>
        public string PetNameString
        {
            get => ModUtils.AddSpacesInCamelCase(_petName);
        }

        /// <summary>
        /// Public getter and setter for CreatureName
        /// </summary>
        public string PetName
        {
            set => _petName = value;
            get => _petName;
        }

        /// <summary>
        /// Public getter and setter for PetDetails
        /// </summary>
        public PetSaver.PetDetails PetSaverDetails
        {
            set => _petSaverDetails = value;
            get => _petSaverDetails;
        }

        /// <summary>
        /// Set up new components
        /// </summary>
        public virtual void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        /// <summary>
        /// Unity Start method
        /// </summary>
        public virtual void Start()
        {
            UpdateActions();
            RegisterNewPet();
            SetMoveMethod();
            SetScale();
        }

        /// <summary>
        /// Sets the GameObject scale
        /// </summary>
        private void SetScale()
        {
            transform.localScale = ScaleFactor;
        }

        /// <summary>
        /// Sets the method of movement based on what components are preset
        /// </summary>
        private void SetMoveMethod()
        {
            _moveOnSurface = GetComponent<MoveOnSurface>();
            if (!_moveOnSurface)
            {
                LogUtils.LogDebug(LogArea.MonoPets, "Pet: No MoveOnSurface component found.");
            }

            _moveOnGround = GetComponent<MoveOnGround>();
            if (!_moveOnGround)
            {
                LogUtils.LogDebug(LogArea.MonoPets, "Pet: No MoveOnGround component found.");
            }

            _simpleMovement = GetComponent<SimpleMovement>();
            if (!_simpleMovement)
            {
                LogUtils.LogDebug(LogArea.MonoPets, "Pet: No SimpleMovement component found.");
            }

            _canMove = _moveOnGround || _moveOnSurface || _simpleMovement;

            if (!_canMove)
            {
                LogUtils.LogDebug(LogArea.MonoPets, "Pet: No ground movement behaviour found. Cannot move to player!");
            }
        }

        /// <summary>
        /// Play a pet animation
        /// </summary>
        public virtual void PlayAnimation()
        {
            if (_animator)
            {
                _animator.SetTrigger("flinch");
            }
            else
            {
                LogUtils.LogDebug(LogArea.MonoPets, "Pet: No animator found, so can't play animation.");
            }
        }

        /// <summary>
        /// Move the pet towards the player location
        /// </summary>
        public void MoveToPlayer()
        {
            if (!_canMove)
            {
                LogUtils.LogDebug(LogArea.MonoPets, "Pet: No movement component found, so can't walk to player");
                return;
            }

            if (_moveOnSurface)
            {
                LogUtils.LogDebug(LogArea.MonoPets, "Pet: Moving via MoveOnSurface");
                _moveOnSurface.walkBehaviour.GoToInternal(Player.main.transform.position,
                    (Player.main.transform.position - transform.position).normalized, _moveOnSurface.moveVelocity);
                return;
            }

            if (_moveOnGround)
            {
                LogUtils.LogDebug(LogArea.MonoPets, "Pet: Moving via MoveOnGround");
                _moveOnGround.swimBehaviour.GoToInternal(Player.main.transform.position,
                    (Player.main.transform.position - transform.position).normalized, _moveOnGround.swimVelocity);
            }

            if (_simpleMovement)
            {
                LogUtils.LogDebug(LogArea.MonoPets, "Pet: Moving via SimpleMovement");
                _simpleMovement.SetDestination(Player.main.transform.position);
            }
        }

        /// <summary>
        /// Rescans the creature for valid actions. Used after adding or removing
        /// new actions.
        /// </summary>
        public void UpdateActions()
        {
            Creature creature = GetComponent<Creature>();
            if (creature)
            {
                creature.ScanCreatureActions();
            }
        }


        /// <summary>
        /// Registers the new pet with the saver and UI list
        /// </summary>
        public void RegisterNewPet()
        {
            _petSaverDetails = Saver.RegisterPet(this);
        }

        /// <summary>
        /// Unregisters the pet with the saver. For example, if the
        /// pet dies
        /// </summary>
        public void UnregisterPet()
        {
            Saver.UnregisterPet(this);
        }

        /// <summary>
        /// Rename the Pet
        /// </summary>
        /// <param name="newName"></param>
        public void RenamePet(string newName)
        {
            PetName = newName;
            _petSaverDetails.PetName = newName;
        }

        /// <summary>
        /// Kills the pet by applying damage
        /// </summary>
        public void Kill()
        {
            LiveMixin liveMixin = GetComponent<LiveMixin>();
            if (liveMixin)
            {
                liveMixin.Kill();
                ErrorMessage.AddMessage(
                    $"{Language.main.Get("Alert_PetDeadFarewell")} {PetNameString}! {Language.main.Get("Alert_PetDeadGoodBoy")} {PetTypeString}!");
            }
        }
    }
}
