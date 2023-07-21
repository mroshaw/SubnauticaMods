using mset;
using System;
using UnityEngine;
using static CreaturePetMod_SN.CreaturePetMod_SNPlugin;

namespace CreaturePetMod_SN.MonoBehaviours
{
    public enum PetCreatureType { CaveCrawler, BloodCrawler, CrabSquid, AlienRobot }
    public enum PetName { Anise, Beethoven, Bitey, Buddy, Cheerio, Clifford, Denali, Fuzzy, Gandalf,
        Grob, Hera, Jasper, Juju, Kovu, Lassie, Lois, Meera, Mochi, Oscar, Picasso, Ranger, Sampson, 
        Shadow, Sprinkles, Stinky, Tobin, Wendy, Zola }


    /// <summary>
    /// MonoBehaviour component providing Pet behaviours and properties
    /// </summary>
    public class Pet : MonoBehaviour
    {
        // Private pet info
        private PetCreatureType _petCreatureType;
        private PetName _petName;
        private MoveOnSurface _moveOnSurface;
        private Animator _animator;

        // Used to keep tabs on saved pets
        private PetSaver.PetDetails _petSaverDetails;

        private bool _isFollowingPlayer = false;

        /// <summary>
        /// Public getter and setter for PetCreatureType
        /// </summary>
        public PetCreatureType PetCreatureType
        {
            set => _petCreatureType = value;
            get => _petCreatureType;
        }

        /// <summary>
        /// Public getter and setter for CreatureName
        /// </summary>
        public PetName PetName
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
        public void Start()
        {
            Log.LogDebug($"Pet: In Pet.Start on parent Game Object: {gameObject.name}");

            _moveOnSurface = GetComponent<MoveOnSurface>();
            if (!_moveOnSurface)
            {
                Log.LogDebug("Pet: No MoveOnSurface component found, so no beckoning.");
            }

            _animator = GetComponent<Animator>();
            if (!_animator)
            {
                Log.LogDebug($"PetHandTarget: No animator found, so no pet animations will play");
            }

            Log.LogDebug($"Pet: Cleaning up components...");
            CleanUpComponents();
            Log.LogDebug($"Pet: Cleaning up components... Done.");

            Log.LogDebug($"Pet: Adding new Pet components...");
            AddNewComponents();
            Log.LogDebug($"Pet: Adding new Pet components... Done.");

            Log.LogDebug($"Pet: Configure components...");
            ConfigureSkyApplier();
            Log.LogDebug($"Pet: Configure components... Done.");


        }

        /// <summary>
        /// Registers the new pet with the saver
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
            Saver.RemovePet(this);
        }

        /// <summary>
        /// Removes unwanted components
        /// </summary>
        private void CleanUpComponents()
        {
            Log.LogDebug($"Pet: Removing Pickupable component...");
            Pickupable pickupable = gameObject.GetComponent<Pickupable>();
            if (pickupable)
            {
                Log.LogDebug($"Pet: Destroying Pickupable component...");
                Destroy(pickupable);
            }
            Log.LogDebug($"Pet: Removing Pickupable component... Done.");
        }

        /// <summary>
        /// Add new pet specific components
        /// </summary>
        private void AddNewComponents()
        {
            Log.LogDebug($"Pet: Adding PetHandTarget component...");
            gameObject.AddComponent<PetHandTarget>();
            Log.LogDebug($"Pet: Adding PetHandTarget component... Done.");
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
                skyApplier = gameObject.AddComponent<SkyApplier>();
            }
            Log.LogDebug($"Pet: Configuring Sky and SkyApplier...");
            skyApplier.SetSky(Skies.BaseInterior);
            Log.LogDebug($"Pet: Configuring Sky and SkyApplier... Done.");
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
                Log.LogDebug("Pet: No animator found, so can't play animation.");
            }
        }

        /// <summary>
        /// Move the pet towards the player location
        /// </summary>
        public void WalkToPlayer()
        {
            if (_moveOnSurface)
            {
                _moveOnSurface.walkBehaviour.GoToInternal(Player.main.transform.position, (Player.main.transform.position - transform.position).normalized, _moveOnSurface.moveVelocity);
            }
            else
            {
                Log.LogDebug("Pet: No MoveOnSurface, so can't walk to player");
            }
        }

        /// <summary>
        /// Pet will follow the player until told to stop
        /// </summary>
        public void FollowPlayer()
        {
            _isFollowingPlayer = true;
        }

        /// <summary>
        /// Stop following the player
        /// </summary>
        public void StopFollowingPlayer()
        {
            _isFollowingPlayer = false;
        }
    }
}
