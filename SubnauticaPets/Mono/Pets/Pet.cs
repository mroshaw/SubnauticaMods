using System.Collections;
using DaftAppleGames.SubnauticaPets.Utils;
using UnityEngine;

namespace DaftAppleGames.SubnauticaPets.Mono.Pets
{

    /// <summary>
    /// MonoBehaviour component providing Pet behaviours and properties.
    /// Acts as a base to be inherited by Creature specific classes
    /// </summary>
    public class Pet : MonoBehaviour
    {
        // Public properties
        public GameObject ParentBaseGameObject { get; set; }

        /// <summary>
        /// The TechType or type of Pet
        /// </summary>
        public TechType TechType => _techTag.type;

        /// <summary>
        /// The PrefabIdentifier. Used to load and re-configure saved pets
        /// </summary>
        public string PrefabId => _prefabIdentifier.Id;

        /// <summary>
        /// Gets a "display friendly" version of the Pet Type for display
        /// </summary>
        public string PetTypeString {
            get
            {
                if (_techTag == null)
                {
                    LogUtils.LogError(LogArea.MonoPets, "Pet _techTag is null!!!");
                    return "Unknown";
                }
                else
                {
                    return _techTag.type.ToString().Substring(0, _techTag.type.ToString().Length - 3).AddSpacesInCamelCase();
                }
            }
    } 

        /// <summary>
        /// Gets a "display friendly" version of the Pet Name
        /// </summary>
        public string PetNameString => PetName.AddSpacesInCamelCase();

        /// <summary>
        /// Public getter and setter for Pet Name
        /// </summary>
        public string PetName { set; get; }

        // Private components
        private MoveOnSurface _moveOnSurface;
        private MoveOnGround _moveOnGround;
        private SimpleMovement _simpleMovement;
        private Animator _animator;
        private TechTag _techTag;
        private PrefabIdentifier _prefabIdentifier;

        private bool _canMove = false;
        
        /// <summary>
        /// Set up new components
        /// </summary>
        public void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            _animator.enabled = true;
            gameObject.DestroyComponentsInChildren<Pickupable>();
#if SUBNAUTICAZERO
            PrefabConfigUtils.CleanNavUpMesh(gameObject);
#endif
            _techTag = GetComponent<TechTag>();
            _prefabIdentifier = GetComponent<PrefabIdentifier>();
        }

        /// <summary>
        /// Unity Start method
        /// </summary>
        public void Start()
        {
            UpdateActions();
            SetMoveMethod();
            CleanUpConstructable();
#if SUBNAUTICA
            LoadPetData();
#endif
        }

        /// <summary>
        /// Wait for data to be loaded, then update if this is a loaded pet
        /// </summary>
        /// <returns></returns>
        public void LoadPetData()
        {
            foreach (PetSaver.PetDetails petDetails in SubnauticaPetsPlugin.LoadedPetDetailsHashSet)
            {
                if (petDetails.PrefabId == PrefabId)
                {
                    LogUtils.LogDebug(LogArea.MonoPets, $"Pet: Found {petDetails.PrefabId}, assigning Pet name");
                    PetName = petDetails.PetName;
                    break;
                }
            }
            SubnauticaPetsPlugin.PetSaver.RegisterPet(this);
        }

        /// <summary>
        /// Write Pet data to the log
        /// </summary>
        /// <param name="identifier"></param>
        private void LogPetData(string identifier)
        {
            LogUtils.LogDebug(LogArea.MonoPets, $"{identifier}: GameObject is: {gameObject.name}, ObjectId is: {gameObject.GetInstanceID()}, " +
                                                $"Transform is: ({gameObject.transform.position.x},{gameObject.transform.position.y}, {gameObject.transform.position.z}), Type is: {PetTypeString}, PrefabId is: {PrefabId}");
        }

        /// <summary>
        /// Clean up Constructable components
        /// </summary>
        private void CleanUpConstructable()
        {
            Constructable constructable = GetComponent<Constructable>();
            if (constructable != null)
            {
                constructable.enabled = false;
            }
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
        public void PlayAnimation()
        {
            if (_animator)
            {
                _animator.SetTrigger("flinch");
            }
            else
            {
                LogUtils.LogError(LogArea.MonoPets, "Pet: No animator found, so can't play animation.");
            }
        }

        /// <summary>
        /// Move the pet towards the player location
        /// </summary>
        public void MoveToPlayer()
        {
            if (!_canMove)
            {
                LogUtils.LogError(LogArea.MonoPets, "Pet: No movement component found, so can't walk to player");
                return;
            }

            if (_moveOnSurface)
            {
                _moveOnSurface.walkBehaviour.GoToInternal(Player.main.transform.position,
                    (Player.main.transform.position - transform.position).normalized, _moveOnSurface.moveVelocity);
                return;
            }

            if (_moveOnGround)
            {
                _moveOnGround.swimBehaviour.GoToInternal(Player.main.transform.position,
                    (Player.main.transform.position - transform.position).normalized, _moveOnGround.swimVelocity);
                return;
            }

            if (_simpleMovement)
            {
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
        /// Listen for the OnKill message
        /// </summary>
        public void OnKill()
        {
            LogUtils.LogDebug(LogArea.MonoPets, "Picked up the OnKill message...");
            ErrorMessage.AddMessage(
                $"{Language.main.Get("Alert_PetDeadFarewell")} {PetNameString}! {Language.main.Get("Alert_PetDeadGoodBoy")} {PetTypeString}!");
            SubnauticaPetsPlugin.PetSaver.UnregisterPet(this);
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
            }
        }
    }
}
