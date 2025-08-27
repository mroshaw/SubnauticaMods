using DaftAppleGames.SubnauticaPets.Extensions;
using System.Collections;
using UnityEngine;

namespace DaftAppleGames.SubnauticaPets.Pets
{
    internal enum PetHappiness { Ecstatic, Happy, Neutral, Sad, Devastated, Dead }
    /// <summary>
    /// MonoBehaviour component providing Pet behaviours and properties.
    /// Can be used as a base to be inherited by Creature specific classes
    /// </summary>
    public class Pet : MonoBehaviour
    {
        // Public properties
        internal Base Base { get; set; }
        internal PetHappiness Happiness { get; private set; }
        internal string BaseId => Base != null ? Base.GetComponent<PrefabIdentifier>().Id : "NO BASE!";

        internal float timeBeforePetNeutral = 1800.0f;
        internal float timeBeforePetSad = 3600.0f;
        internal float timeBeforePetDevastated = 5400.0f;
        
        private float _timeSinceLastInteraction;

        /// <summary>
        /// The TechType or type of Pet
        /// </summary>
        public TechType TechType => _techTag.type;

        /// <summary>
        /// The PrefabIdentifier. Used to load and re-configure saved pets
        /// </summary>
        public string PrefabId => _prefabIdentifier.Id;

        public bool IsDead { get; private set; } = false;

        /// <summary>
        /// Gets a "display friendly" version of the Pet Type for display
        /// </summary>
        public string PetTypeString
        {
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

        private static readonly int Dead = Animator.StringToHash("dead");

        /// <summary>
        /// Gets a "display friendly" version of the Pet Name
        /// </summary>
        public string PetNameString => PetName.AddSpacesInCamelCase();

        /// <summary>
        /// Public getter and setter for Pet Name
        /// </summary>
        public string PetName { set; get; }

        // Private components
        private PetStateController _petStateController;
        private MoveOnSurface _moveOnSurface;
        private MoveOnGround _moveOnGround;
        private Animator _animator;
        private PetAnimator _petAnimator;
        private FMOD_CustomEmitter _fmodEmitter;
        private TechTag _techTag;
        private PrefabIdentifier _prefabIdentifier;
        private SkyApplier _skyApplier;

        private readonly RaycastHit[] _baseCheckCache = new RaycastHit[10];
        private Ray _rayOrigin = new Ray();
        private BaseRoot _baseRootCache = new BaseRoot();

        private bool _canMove = false;

        private const float DelayBeforeDestroy = 10.0f;

        private LiveMixin _liveMixin;
        private Rigidbody _rigidBody;

        private void OnDisable()
        {
            SubnauticaPetsPlugin.PetSaver.UnregisterPet(this);
        }

        /// <summary>
        /// Set up new components
        /// </summary>
        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            _animator.enabled = true;
            _techTag = GetComponent<TechTag>();
            _prefabIdentifier = GetComponent<PrefabIdentifier>();
            _liveMixin = GetComponent<LiveMixin>();
            _skyApplier = GetComponent<SkyApplier>();
            _fmodEmitter = GetComponent<FMOD_CustomEmitter>();
            _rigidBody = GetComponent<Rigidbody>();
            _petAnimator = GetComponent<PetAnimator>();
        }

        /// <summary>
        /// Unity Start method
        /// </summary>
        private void Start()
        {
            // Need to avoid doing anything for the prefab
            if (!gameObject.name.Contains("Clone"))
            {
                LogUtils.LogDebug(LogArea.Prefabs, $"Start called on Pet Prefab: {gameObject.name}");
                return;
            }

            LogUtils.LogDebug(LogArea.Prefabs, $"In Pet Start for: {gameObject.name}");

            UpdateActions();
            SetMoveMethod();
            StartCoroutine(CleanUpComponentsAfterDelay());
            LoadPetData();
            DeriveBase();
            SubnauticaPetsPlugin.PetSaver.RegisterPet(this);
        }
        
        private void SetPetHappiness()
        {
            _timeSinceLastInteraction += Time.deltaTime;

            if (_timeSinceLastInteraction < timeBeforePetNeutral)
            {
                Happiness = PetHappiness.Happy;
            }
            else if (_timeSinceLastInteraction < timeBeforePetSad)
            {
                Happiness = PetHappiness.Neutral;
            }
            else if (_timeSinceLastInteraction < timeBeforePetDevastated)
            {
                Happiness = PetHappiness.Sad;
            }
            else
            {
                Happiness = PetHappiness.Devastated;
            }
        }

        private void ParentToBase()
        {
            if (!Base || transform.parent == Base.transform)
            {
                return;
            }

            LogUtils.LogDebug(LogArea.MonoPets, $"Fixing parent for {PetName}");
            transform.SetParent(base.transform);
            _skyApplier.SetSky(Skies.BaseInterior);
        }

        private void DeriveBase()
        {
            LogUtils.LogDebug(LogArea.MonoPets, $"In DeriveBase for {PetName}");
            if (Base)
            {
                LogUtils.LogDebug(LogArea.MonoPets, $"Base already set!");
                return;
            }

            LogUtils.LogDebug(LogArea.MonoPets, $"Base not set");

            if (transform.parent)
            {
                LogUtils.LogDebug(LogArea.MonoPets, $"Looking for BaseRoot in parent");
                Base = transform.parent.GetComponent<Base>();
                if (Base)
                {
                    LogUtils.LogDebug(LogArea.MonoPets, $"Setting Base to: {Base.gameObject.name}...");
                    return;
                }
            }

            LogUtils.LogDebug(LogArea.MonoPets, "Looking for Base via RayCast...");

            _rayOrigin = new Ray(transform.position, transform.up);
            int numHits = Physics.RaycastNonAlloc(_rayOrigin, _baseCheckCache, maxDistance: 10.0f);

            LogUtils.LogDebug(LogArea.MonoPets, $"RayCast hit {numHits} colliders");

            for (int curHit = 0; curHit < numHits; curHit++)
            {
                LogUtils.LogDebug(LogArea.MonoPets, $"Looking in {_baseCheckCache[curHit].collider.gameObject.name}.");

                Base = _baseCheckCache[curHit].collider.transform.parent.GetComponentInChildren<Base>(true);

                if (Base)
                {
                    LogUtils.LogDebug(LogArea.MonoPets, $"Found BaseRoot in {Base.gameObject.name}");
                    return;
                }
            }
            LogUtils.LogError(LogArea.MonoPets, $"Can't find Base for Pet {PetName}!");
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
        /// Clean up Constructable components. This prevents the Pet from being targetted by the Builder for "Deconstruction"
        /// </summary>
        private void CleanUpComponents()
        {
            gameObject.DestroyComponentsInChildren<Constructable>();
            gameObject.DestroyComponentsInChildren<Pickupable>();
        }

        private IEnumerator CleanUpComponentsAfterDelay()
        {
            yield return new WaitForSeconds(2.0f);
            CleanUpComponents();
        }

        /// <summary>
        /// Sets the method of movement based on what components are preset
        /// </summary>
        private void SetMoveMethod()
        {
            _moveOnSurface = GetComponent<MoveOnSurface>();
            if (!_moveOnSurface)
            {
                // LogUtils.LogDebug(LogArea.MonoPets, "Pet: No MoveOnSurface component found.");
            }

            _moveOnGround = GetComponent<MoveOnGround>();
            if (!_moveOnGround)
            {
                // LogUtils.LogDebug(LogArea.MonoPets, "Pet: No MoveOnGround component found.");
            }

            _petStateController = GetComponent<PetStateController>();
            if (!_petStateController)
            {
                // LogUtils.LogDebug(LogArea.MonoPets, "Pet: No PetStateController component found.");
            }

            _canMove = _moveOnGround || _moveOnSurface || _petStateController;

            if (!_canMove)
            {
                LogUtils.LogDebug(LogArea.MonoPets, "Pet: No ground movement behaviour found. Cannot move to player!");
            }
        }

        internal void PlaySound()
        {
            if (_fmodEmitter)
            {
                _fmodEmitter.Play();
            }
        }

        /// <summary>
        /// Play a pet animation
        /// </summary>
        internal void PlayAnimation()
        {
            if (!_petAnimator && _animator)
            {
                _animator.SetTrigger("flinch");
                PlaySound();
            }
            else if (_petAnimator)
            {
                _petAnimator.PlayRandomBodyAnim(true);
                _petAnimator.PlayRandomFaceAnim();
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

            if (_petStateController)
            {
                _petStateController.MoveToPosition(Player.main.transform.position);
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
            if (IsDead)
            {
                return;
            }

            IsDead = true;
            _rigidBody.isKinematic = true;
            if (_petStateController)
            {
                _petStateController.Kill();
            }

            Happiness = PetHappiness.Dead;

            SubnauticaPetsPlugin.PetSaver.UnregisterPet(this);
            LogUtils.LogDebug(LogArea.MonoPets, $"Picked up the OnKill message in {gameObject.name}");

            if (!string.IsNullOrEmpty(PetNameString))
            {
                ErrorMessage.AddMessage($"{Language.main.Get("Alert_PetDeadFarewell")} {PetNameString}! {Language.main.Get("Alert_PetDeadGoodBoy")} {PetTypeString}!");
            }

            // Destroy GameObject after delay
            StartCoroutine(DestroyAfterDelay());
        }

        /// <summary>
        /// Kills the pet by applying damage
        /// </summary>
        public void Kill()
        {
            if (_liveMixin)
            {
                _liveMixin.Kill();
            }
        }

        private IEnumerator DestroyAfterDelay()
        {
            yield return new WaitForSeconds(DelayBeforeDestroy);
            Destroy(this.gameObject);
        }
        
        /// <summary>
        /// Find and Kill all pets. For use to clear out all pets in case
        /// of some sort of catastrophic failure.
        /// </summary>
        public static void KillAllPets()
        {
            foreach (Pet pet in GameObject.FindObjectsOfType<Pet>())
            {
                pet.Kill();
            }
        }
    }
}