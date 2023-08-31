using DaftAppleGames.SubnauticaPets.Utils;
using UnityEngine;
using static DaftAppleGames.SubnauticaPets.SubnauticaPetsPlugin;

namespace DaftAppleGames.SubnauticaPets.MonoBehaviours.Pets
{

    // Define available Pet types, depending on which game the mod is compiled for
#if SUBNAUTICA
    public enum PetCreatureType { CaveCrawler, BloodCrawler, CrabSquid, AlienRobot }
#endif

#if SUBNAUTICAZERO
    public enum PetCreatureType { SnowstalkerBaby, PenglingBaby, PenglingAdult, Pinnicarid, BlueTrivalve, YellowTrivalve }
#endif

    public enum PetName { Anise, Beethoven, Bitey, Buddy, Cheerio, Clifford, Denali, Fuzzy, Gandalf,
        Grob, Hera, Jasper, Juju, Kovu, Lassie, Lois, Meera, Mochi, Oscar, Picasso, Ranger, Sampson, 
        Shadow, Sprinkles, Stinky, Tobin, Wendy, Zola }

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
            get => ModUtils.AddSpacesInCamelCase(_petName.ToString());
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
        /// Unity Start method
        /// </summary>
        public virtual void Start()
        {
            Log.LogDebug($"Pet: In Pet.Start on parent Game Object: {gameObject.name}");

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

            _canMove = _moveOnGround || _moveOnSurface;

            if (!_canMove)
            {
                Log.LogDebug("Pet: No ground movement behaviour found. Cannot move to player!");
            }

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
        }

        /// <summary>
        /// Updates pet specific components
        /// </summary>
        public virtual void UpdateComponents()
        {
            Log.LogDebug("Pet: Configuring Sky and SkyApplier...");
            ConfigureSkyApplier();
            Log.LogDebug("Pet: Configuring Sky and SkyApplier... Done.");
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
            _creature.ScanCreatureActions();
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

            Log.LogError("Pet: ConfigureSkyApplier setting SkyApplier Sky.");
            skyApplier.SetSky(Skies.BaseInterior);

            Log.LogError("Pet: ConfigureSkyApplier updating renderers.");
            Renderer[] creatureRenderers = gameObject.GetComponentsInChildren<Renderer>();
            if (creatureRenderers.Length > 0)
            {
                skyApplier.renderers = creatureRenderers;
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
                ErrorMessage.AddMessage($"Goodbye, {PetNameString}! You've been the goodest {PetTypeString}!");
            }
        }

        /// <summary>
        /// Pet is "Born"
        /// </summary>
        public void Born()
        {
            ErrorMessage.AddMessage($"Welcome to your new {PetTypeString}, {PetNameString}!");
        }
    }
}
