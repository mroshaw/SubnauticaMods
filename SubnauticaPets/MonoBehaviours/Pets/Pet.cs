using DaftAppleGames.SubnauticaPets.Utils;
using UnityEngine;
using static DaftAppleGames.SubnauticaPets.SubnauticaPetsPlugin;

namespace DaftAppleGames.SubnauticaPets.MonoBehaviours.Pets
{

    // Define available Pet types, depending on which game the mod is compiled for
#if SUBNAUTICA
    public enum PetCreatureType { CaveCrawler, BloodCrawler, CrabSquid, AlienRobot, Cat }
#endif

#if SUBNAUTICAZERO
    public enum PetCreatureType { SnowstalkerBaby, PenglingBaby, PenglingAdult, Pinnicarid, BlueTrivalve, YellowTrivalve, Cat }
#endif

    /// <summary>
    /// MonoBehaviour component providing Pet behaviours and properties.
    /// Acts as a base to be inherited by Creature specific classes
    /// </summary>
    public abstract class Pet : MonoBehaviour, IPet
    {
        // Public properties
        
        // Scale factor is set as the LocalScale of spawned gameobject
        // across all axis
        public abstract float ScaleFactor { get; }

        // Private pet info
        private PetCreatureType _petCreatureType;
        private string _petName;
        private MoveOnSurface _moveOnSurface;
        private MoveOnGround _moveOnGround;
        private SimpleMovement _simpleMovement;
        private Animator _animator;
        private Creature _creature;

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

        public virtual void Awake()
        {

        }

        /// <summary>
        /// Unity Start method
        /// </summary>
        public virtual void Start()
        {
            Log.LogDebug($"Pet: In Pet.Start on parent Game Object: {gameObject.name}");
            _creature = gameObject.GetComponent<Creature>();
            if (_creature)
            {
                _animator = _creature.GetAnimator();
            }
            else
            {
                Log.LogDebug("PetHandTarget: Couldn't find Creature component. Unable to find Animator.");
            }

            if (!_animator)
            {
                Log.LogDebug("PetHandTarget: No animator found, so no pet animations will play");
            }

            Log.LogDebug($"Pet: Setting Pet Scale to {ScaleFactor}...");
            SetScale();
            Log.LogDebug($"Pet: Setting Pet Scale to {ScaleFactor}...");

            Log.LogDebug("Pet: Cleaning up components...");
            RemoveComponents();
            Log.LogDebug("Pet: Cleaning up components... Done.");

            Log.LogDebug("Pet: Adding new Pet components...");
            AddComponents();
            Log.LogDebug("Pet: Adding new Pet components... Done.");

            Log.LogDebug("Pet: Configure components...");
            UpdateComponents();
            Log.LogDebug("Pet: Configure components... Done.");

            Log.LogDebug("Pet: Refreshing creature actions...");
            UpdateActions();
            Log.LogDebug("Pet: Refreshing creature actions... Done.");

            Log.LogDebug("Pet: Adding Pet to Global List...");
            Saver.AddPetToList(this);
            Log.LogDebug("Pet: Adding Pet to Global List... Done.");

            Log.LogDebug("Pet: Set MoveMethod...");
            SetMoveMethod();
            Log.LogDebug("Pet: Set MoveMethod... Done.");
        }

        /// <summary>
        /// Sets the method of movement based on what components are preset
        /// </summary>
        private void SetMoveMethod()
        {
            _moveOnSurface = GetComponent<MoveOnSurface>();
            if (!_moveOnSurface)
            {
                Log.LogDebug("Pet: No MoveOnSurface component found.");
            }

            _moveOnGround = GetComponent<MoveOnGround>();
            if (!_moveOnGround)
            {
                Log.LogDebug("Pet: No MoveOnGround component found.");
            }

            _simpleMovement = GetComponent<SimpleMovement>();
            if (!_simpleMovement)
            {
                Log.LogDebug("Pet: No SimpleMovement component found.");
            }

            _canMove = _moveOnGround || _moveOnSurface || _simpleMovement;

            if (!_canMove)
            {
                Log.LogDebug("Pet: No ground movement behaviour found. Cannot move to player!");
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
        /// Removes unwanted components
        /// </summary>
        public virtual void RemoveComponents()
        {
            ModUtils.DestroyComponentsInChildren<Pickupable>(gameObject);
            Log.LogDebug("Pet: Destroying components... Done.");
        }

        /// <summary>
        /// Add new pet specific components
        /// </summary>
        public virtual void AddComponents()
        {
            Log.LogDebug("Pet: Adding PetHandTarget component...");
            gameObject.AddComponent<PetHandTarget>();
            Log.LogDebug("Pet: Adding PetHandTarget component... Done.");

            Log.LogDebug("Pet: Adding RigidBody component...");
            AddRigidBody();
            Log.LogDebug("Pet: Adding RigidBody component... Done.");
        }

        /// <summary>
        /// Updates pet specific components
        /// </summary>
        public virtual void UpdateComponents()
        {
            Log.LogDebug("Pet: Configuring Sky and SkyApplier...");
            ConfigureSkyApplier();
            Log.LogDebug("Pet: Configuring Sky and SkyApplier... Done.");
            Log.LogDebug("Pet: Enabling Animator...");
            this.GetComponentInChildren<Animator>().enabled = true;
            Log.LogDebug("Pet: Enabling Animator... Done.");

        }

        /// <summary>
        /// Sets the pet scale
        /// </summary>
        private void SetScale()
        {
            gameObject.transform.localScale = new Vector3(ScaleFactor, ScaleFactor, ScaleFactor);
        }

        /// <summary>
        /// Rescans the creature for valid actions. Used after adding or removing
        /// new actions.
        /// </summary>
        private void UpdateActions()
        {
            if (_creature)
            {
                _creature.ScanCreatureActions();
            }
        }

        /// <summary>
        /// Adds a RigidBody, if not one already
        /// </summary>
        private void AddRigidBody()
        {
            Rigidbody rigidbody = gameObject.GetComponent<Rigidbody>();
            if (rigidbody == null)
            {
                rigidbody = gameObject.AddComponent<Rigidbody>();
                rigidbody.mass = 0.5f;
                rigidbody.useGravity = true;
                rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
                rigidbody.isKinematic = false;
            }
        }

        /// <summary>
        /// Configures the Sky and SkyApplier, to ensure
        /// creature mesh shaders don't look "dull".
        /// </summary>
        private void ConfigureSkyApplier()
        {
            SkyApplier skyApplier = gameObject.GetComponent<SkyApplier>();
            if (!skyApplier)
            {
                Log.LogError("Pet: ConfigureSkyApplier added SkyApplier component.");
                skyApplier = gameObject.AddComponent<SkyApplier>();
            }

            // Log.LogError("Pet: ConfigureSkyApplier setting SkyApplier Sky.");
            // skyApplier.SetSky(Skies.BaseInterior);

            Log.LogDebug("Pet: ConfigureSkyApplier updating renderers...");
            Renderer[] creatureRenderers = gameObject.GetComponentsInChildren<Renderer>(true);
            Log.LogDebug($"Pet: ConfigureSkyApplier found {creatureRenderers.Length} renderers...");
            if (creatureRenderers.Length > 0)
            {
                skyApplier.renderers = creatureRenderers;
            }
            Log.LogDebug("Pet: ConfigureSkyApplier updating renderers... Done.");

            skyApplier.anchorSky = Skies.BaseInterior;
            GameObject environment = SkyApplier.GetEnvironment(gameObject, skyApplier.anchorSky);
            if (environment == null)
            {
                Log.LogDebug("Pet: ConfigureSkyApplier got null back from SkyApplier.GetEnvironment!");
            }
            else
            {
                skyApplier.GetAndApplySkybox(environment);
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
                Log.LogDebug("Pet: No animator found, so can't play animation.");
            }
        }

        /// <summary>
        /// Move the pet towards the player location
        /// </summary>
        public void MoveToPlayer()
        {
            if (!_canMove)
            {
                Log.LogDebug("Pet: No movement component found, so can't walk to player");
                return;
            }

            if (_moveOnSurface)
            {
                Log.LogDebug("Pet: Moving via MoveOnSurface");
                _moveOnSurface.walkBehaviour.GoToInternal(Player.main.transform.position, (Player.main.transform.position - transform.position).normalized, _moveOnSurface.moveVelocity);
                return;
            }

            if (_moveOnGround)
            {
                Log.LogDebug("Pet: Moving via MoveOnGround");
                _moveOnGround.swimBehaviour.GoToInternal(Player.main.transform.position, (Player.main.transform.position - transform.position).normalized, _moveOnGround.swimVelocity);
            }

            if (_simpleMovement)
            {
                Log.LogDebug("Pet: Moving via SimpleMovement");
                _simpleMovement.SetDestination(Player.main.transform.position);
            }
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
                ErrorMessage.AddMessage($"{Language.main.Get("Alert_PetDeadFarewell")} {PetNameString}! {Language.main.Get("Alert_PetDeadGoodBoy")} {PetTypeString}!");
            }
        }

        /// <summary>
        /// Pet is "Born"
        /// </summary>
        public void Born()
        {
            ErrorMessage.AddMessage($"{Language.main.Get("Alert_PetBorn")} {PetTypeString}, {PetNameString}!");
        }
    }
}
