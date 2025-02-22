using System.Linq;
using UnityEngine;
using static DaftAppleGames.SeaTruckFishScoopMod_BZ.SeaTruckFishScoopPluginBz;

namespace DaftAppleGames.SeaTruckFishScoopMod_BZ.MonoBehaviours
{
    public class SeaTruckFishScoop : MonoBehaviour
    {
        private bool _isOn;
        private SeaTruckMotor _mainMotor;

        // Static FMODAsset for playing sounds
        private static readonly FMODAsset SoundsToPlay = ScriptableObject.CreateInstance<FMODAsset>();
        private static readonly string FishScoopPowerOnSoundPath = "event:/sub/cyclops/start";
        private static readonly string FishScoopPowerOffSoundPath = "event:/sub/base/power_off";
        private static readonly string AquariumPurgeSoundPath = "event:/player/bubbles";

        /// <summary>
        /// Initialise the scoop
        /// </summary>
        public void Start()
        {
            _isOn = false;
            _mainMotor = GetComponentInParent<SeaTruckMotor>();
            if(!_mainMotor)
            {
                Log.LogDebug("FishScoop Start: Could not find SeaTruckMotor!");
            }
        }

        /// <summary>
        /// Toggle the Fish Scoop on and off
        /// </summary>
        internal void ToggleScoop()
        {
            Log.LogDebug($"Toggling fish scoop from: {_isOn}...");

            // If we're not in the SeaTruck, call it a day
            if (!_mainMotor)
            {
                return;
            }

            // Can only turn on the scoop in the SeaTruck
            if(!_mainMotor.IsPiloted())
            {
                return;
            }

            // Check if we have any aquarium modules attached
            if (!IsAquariumAttached())
            {
                ErrorMessage.AddMessage($"No aquariums attached!");
                return;
            }

            // Toggle state
            if (_isOn)
            {
                _isOn = false;
                ErrorMessage.AddMessage($"Fish scoop DISABLED");
                SoundsToPlay.path = FishScoopPowerOffSoundPath;
                FMODUWE.PlayOneShot(SoundsToPlay, _mainMotor.transform.position);
            }
            else
            {
                _isOn = true;
                ErrorMessage.AddMessage($"Fish scoop ENABLED");
                SoundsToPlay.path = FishScoopPowerOnSoundPath;
                FMODUWE.PlayOneShot(SoundsToPlay, _mainMotor.transform.position);
            }
        }

        /// <summary>
        /// Attempt to scoop the given GameObject into an attached aquarium
        /// </summary>
        public bool Scoop(GameObject objectToScoop)
        {
            // Is this thing on?
            if(!_isOn)
            {
                return false;
            }

            // Let's see if what took the damage was a compatible aquarium fish
            if (!IsValidObject(objectToScoop))
            {
                return false;
            }

            // Check if seatruck is being piloted and whether or not we're allowed to scoop
            bool isPiloted = _mainMotor.IsPiloted();
            if (!isPiloted && !ConfigFile.ScoopWhilePiloting)
            {
                return false;
            }

            // Check if static against the config options
            float velicityMagnitude = _mainMotor.useRigidbody.velocity.magnitude;
            if ((velicityMagnitude == 0.0f) && !ConfigFile.ScoopWhileStatic)
            {
                 return false;
            }

            // We've passed our checks, now try to add the fish
            SeaTruckFishScoopPluginBz.Log.LogDebug("Taker is a supported fish");
            bool fishAdded = AddFishToFreeAquarium(objectToScoop);
            return fishAdded;
        }

        /// <summary>
        /// Is the object hit valid for inclusion in the Aquarium?
        /// </summary>
        private bool IsValidObject(GameObject takerGameObject)
        {
            Log.LogDebug("In IsValidObject");
            if (!takerGameObject.GetComponent<AquariumFish>())
            {
                Log.LogDebug("IsValidObject: Not an AquariumFish. No Scoop.");
                return false;
            }
            WaterParkCreature waterParkCreature = takerGameObject.GetComponent<WaterParkCreature>();
            if (waterParkCreature && waterParkCreature.IsInsideWaterPark())
            {
                Log.LogDebug("IsValidObject: Target IsInsideWaterPark. No Scoop.");
                return false;
            }
            Log.LogDebug("IsValidObject: Target IsInsideWaterPark. No Scoop.");
            return true;
        }

        /// <summary>
        /// Purge all aquariums attached to the main Seatruck
        /// </summary>
        internal void PurgeAquariums()
        {
            // Check if the SeaTruck being piloted is attached to this scoop
            if(!_mainMotor.IsPiloted())
            {
                return;
            }

            // Check if we have any aquarium modules attached
            if (!IsAquariumAttached())
            {
                Log.LogDebug($"Couldn't find any Aquariums!");
                ErrorMessage.AddMessage($"No aquariums attached!");
                return;
            }

            // Checks all done, we can purge the modules
            SeaTruckAquarium[] seaTruckAquariums = _mainMotor.GetComponentsInChildren<SeaTruckAquarium>();
            Log.LogDebug($"Found {seaTruckAquariums.Length} aquarium modules");
            SoundsToPlay.path = AquariumPurgeSoundPath;
            FMODUWE.PlayOneShot(SoundsToPlay, _mainMotor.transform.position);
            foreach (SeaTruckAquarium seaTruckAquarium in seaTruckAquariums)
            {
                PurgeFishFromAquarium(seaTruckAquarium);
                Log.LogDebug($"Purged aquarium: {seaTruckAquarium.name}");
            }
            ErrorMessage.AddMessage($"All aquariums purged!");
        }

        /// <summary>
        /// Returns true if at least one aquarium module is attached to the SeaTruckMotor
        /// </summary>
        private bool IsAquariumAttached()
        {
            SeaTruckAquarium[] seaTruckAquariums = _mainMotor.GetComponentsInChildren<SeaTruckAquarium>();
            Log.LogDebug($"Found {seaTruckAquariums.Length} aquarium modules");
            // Check to see if there are any aquariums
            if (seaTruckAquariums.Length == 0)
            {
                Log.LogDebug("No aquariums found.");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Adds the specified fish to an aquarium attached to the SeaTruckMotor
        /// </summary>
        private bool AddFishToFreeAquarium(GameObject fish)
        {
            // We hit a supported fish with our SeaTruck cab. Iterate over all Aquarium modules and add the fish to
            // the first one with space
            SeaTruckAquarium[] seaTruckAquariums = _mainMotor.GetComponentsInChildren<SeaTruckAquarium>();
            Log.LogDebug($"Found {seaTruckAquariums.Length} aquarium modules");

            // Check to see if there are any aquariums
            if (seaTruckAquariums.Length == 0)
            {
                Log.LogDebug("No aquariums found.");
                return false;
            }

            foreach (SeaTruckAquarium seaTruckAquarium in seaTruckAquariums)
            {
                if (AddFishToAquarium(seaTruckAquarium, fish))
                {
                    string friendlyFishName = GetFriendlyName(fish.name);
                    Log.LogDebug($"Fish successfully added {fish.name} as {friendlyFishName}");
                    ErrorMessage.AddMessage($"Fish scoop successful! Added {friendlyFishName}");
                    return true;
                }
                Log.LogDebug($"Unable to add fish to this aquarium ({seaTruckAquarium.name}). Likely full or fish is already in one.");
            }
            Log.LogDebug("No free aquariums!");
            ErrorMessage.AddMessage($"Aquariums are full. Fish scoop failed!");
            return false;
        }

        /// <summary>
        /// Removes all fish from the specified Aquarium module
        /// </summary>
        private void PurgeFishFromAquarium(SeaTruckAquarium seaTruckAquarium)
        {
            // Get all creatures / aquariumfish
            ItemsContainer container = seaTruckAquarium.storageContainer.container;

            // Release around the SeaTruck
            Vector3 fishPosition = _mainMotor.transform.position + (_mainMotor.transform.forward * 1.0f) + (_mainMotor.transform.up * 4.0f);
            Log.LogDebug($"Dropping fish in front of MainMotor at: {fishPosition}");

            Vector3 purgeVelocity = _mainMotor.transform.up * 2.0f;

            // Allows us to amend while iterating
            foreach (InventoryItem fishItem in container.ToList())
            {
                Pickupable fishPickupable = fishItem.item;
                Vector3 randomFishPosition = fishPosition + (Random.Range(0, 0.5f) * _mainMotor.transform.forward) + (Random.Range(0, 0.5f) * _mainMotor.transform.up);
                Log.LogDebug($"Dropping {fishItem.item} at: {randomFishPosition}");
                fishPickupable.Drop(randomFishPosition, purgeVelocity, false);

                // Remove from aquarium container
                container.RemoveItem(fishPickupable, true);
                Log.LogDebug($"Removed {fishPickupable.name}");
            }
        }

        /// <summary>
        /// Returns a "user friendly" name for the fish caught
        /// </summary>
        private string GetFriendlyName(string fishName)
        {
            return (fishName.Replace("(Clone)", ""));
        }

        /// <summary>
        /// Add our fish to the chosen Aquarium
        /// </summary>
        private static bool AddFishToAquarium(SeaTruckAquarium seaTruckAquarium, GameObject auquariumFish)
        {
            Pickupable pickupable = auquariumFish.GetComponent<Pickupable>();

            if (seaTruckAquarium.storageContainer.container.HasRoomFor(pickupable))
            {
                Utils.PlayFMODAsset(seaTruckAquarium.collectSound, auquariumFish.transform, 20f);
                pickupable.Initialize();
                InventoryItem item = new InventoryItem(pickupable);
                seaTruckAquarium.storageContainer.container.UnsafeAdd(item);
                return true;
            }
            return false;
        }
    }
}