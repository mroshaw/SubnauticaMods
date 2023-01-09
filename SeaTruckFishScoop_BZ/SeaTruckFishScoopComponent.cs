﻿using BepInEx.Configuration;
using UnityEngine;

namespace Mroshaw.SeaTruckFishScoopMod_BZ
{
    public class SeaTruckFishScoopComponent : MonoBehaviour
    {
        private KeyboardShortcut _toggleScoopKeyboardShortcut;
        private KeyboardShortcut _purgeKeyboardShortcut;
        private bool _isOn;
        private SeaTruckMotor _mainMotor;

        // Static FMODAsset for playing sounds
        private static readonly FMODAsset SoundsToPlay = ScriptableObject.CreateInstance<FMODAsset>();
        private static readonly string fishScoopPowerOnSoundPath = "event:/sub/cyclops/start";
        private static readonly string fishScoopPowerOffSoundPath = "event:/sub/base/power_off";
        private static readonly string aquariumPurgeSoundPath = "event:/player/bubbles";

        /// <summary>
        /// Initialise the scoop
        /// </summary>
        public void Start()
        {
            _toggleScoopKeyboardShortcut = SeaTruckFishScoopPlugin_BZ.ToggleScoopKeyboardShortcut.Value;
            _purgeKeyboardShortcut = SeaTruckFishScoopPlugin_BZ.ReleaseAllKeyboardShortcut.Value;
            _isOn = false;
            _mainMotor = GetComponentInParent<SeaTruckMotor>();
            if(!_mainMotor)
            {
                SeaTruckFishScoopPlugin_BZ.Log.LogDebug("FishScoop Start: Could not find SeaTruckMotor!");
            }
        }

        /// <summary>
        /// Check for scoop keypresses
        /// </summary>
        public void Update()
        {
            // Check for "toggle fish scoop" keypress
            if (_toggleScoopKeyboardShortcut.IsDown())
            {
                SeaTruckFishScoopPlugin_BZ.Log.LogDebug($"Toggle keypress detected");
                // Only toggle when pilotinbg Seatruck
                if (!Player.main.IsPilotingSeatruck())
                {
                    SeaTruckFishScoopPlugin_BZ.Log.LogDebug("Toggle: Not piloting. Abort.");
                    return;
                }

                // Toggle scoop
                ToggleScoop();
            }

            // Check for "purge aquariums" keypress
            if (_purgeKeyboardShortcut.IsDown())
            {
                SeaTruckFishScoopPlugin_BZ.Log.LogDebug("Purge keypress detected");
                // Only allow when pilotinbg Seatruck
                if (!Player.main.IsPilotingSeatruck())
                {
                    SeaTruckFishScoopPlugin_BZ.Log.LogDebug("Purge: Not piloting. Abort.");
                    return;
                }
                SeaTruckFishScoopPlugin_BZ.Log.LogDebug("Attempting to purge Aquariums...");
                PurgeAquariums();
                SeaTruckFishScoopPlugin_BZ.Log.LogDebug("Aquariums purged!");
            }
        }

        /// <summary>
        /// Toggle the Fish Scoop on and off
        /// </summary>
        private void ToggleScoop()
        {
            SeaTruckFishScoopPlugin_BZ.Log.LogDebug($"Toggling fish scoop from: {_isOn}...");

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
                SoundsToPlay.path = fishScoopPowerOffSoundPath;
                FMODUWE.PlayOneShot(SoundsToPlay, _mainMotor.transform.position);
            }
            else
            {
                _isOn = true;
                ErrorMessage.AddMessage($"Fish scoop ENABLED");
                SoundsToPlay.path = fishScoopPowerOnSoundPath;
                FMODUWE.PlayOneShot(SoundsToPlay, _mainMotor.transform.position);
            }
        }

        /// <summary>
        /// Attempt to scoop the given GameObject into an attached aquarium
        /// </summary>
        /// <param name="objectToScoop"></param>
        /// <returns></returns>
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
            if (!isPiloted && !SeaTruckFishScoopPlugin_BZ.ScoopWhileNotPiloting.Value)
            {
                return false;
            }

            // Check if static against the config options
            float velicityMagnitude = _mainMotor.useRigidbody.velocity.magnitude;
            if ((velicityMagnitude == 0.0f) && !SeaTruckFishScoopPlugin_BZ.ScoopWhileStatic.Value)
            {
                 return false;
            }

            // We've passed our checks, now try to add the fish
            SeaTruckFishScoopPlugin_BZ.Log.LogDebug("Taker is a supported fish");
            bool fishAdded = AddFishToFreeAquarium(objectToScoop);
            return fishAdded;
        }

        /// <summary>
        /// Is the object hit valid for inclusion in the Aquarium?
        /// </summary>
        /// <param name="takerGameObject"></param>
        /// <returns></returns>
        private bool IsValidObject(GameObject takerGameObject)
        {
            if (!takerGameObject.GetComponent<AquariumFish>())
            {
                return false;
            }
            WaterParkCreature waterParkCreature = takerGameObject.GetComponent<WaterParkCreature>();
            if (waterParkCreature && waterParkCreature.IsInsideWaterPark())
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Purge all aquariums attached to the main Seatruck
        /// </summary>
        private void PurgeAquariums()
        {

            // Check if the SeaTruck being piloted is attached to this scoop
            if(!_mainMotor.IsPiloted())
            {
                return;
            }

            // Check if we have any aquarium modules attached
            if (!IsAquariumAttached())
            {
                SeaTruckFishScoopPlugin_BZ.Log.LogDebug($"Couldn't find any Aquariums!");
                ErrorMessage.AddMessage($"No aquariums attached!");
                return;
            }

            // Checks all done, we can purge the modules
            SeaTruckAquarium[] seaTruckAquariums = _mainMotor.GetComponentsInChildren<SeaTruckAquarium>();
            SeaTruckFishScoopPlugin_BZ.Log.LogDebug($"Found {seaTruckAquariums.Length} aquarium modules");
            SoundsToPlay.path = aquariumPurgeSoundPath;
            FMODUWE.PlayOneShot(SoundsToPlay, _mainMotor.transform.position);
            foreach (SeaTruckAquarium seaTruckAquarium in seaTruckAquariums)
            {
                PurgeFishFromAquarium(seaTruckAquarium);
                SeaTruckFishScoopPlugin_BZ.Log.LogDebug($"Purged aquarium: {seaTruckAquarium.name}");
            }
            ErrorMessage.AddMessage($"All aquariums purged!");
        }

        /// <summary>
        /// Returns true if at least one aquarium module is attached to the SeaTruckMotor
        /// </summary>
        /// <param name="mainMotor"></param>
        /// <returns></returns>
        private bool IsAquariumAttached()
        {
            SeaTruckAquarium[] seaTruckAquariums = _mainMotor.GetComponentsInChildren<SeaTruckAquarium>();
            SeaTruckFishScoopPlugin_BZ.Log.LogDebug($"Found {seaTruckAquariums.Length} aquarium modules");
            // Check to see if there are any aquariums
            if (seaTruckAquariums.Length == 0)
            {
                SeaTruckFishScoopPlugin_BZ.Log.LogDebug("No aquariums found.");
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Adds the specified fish to an aquarium attached to the SeaTruckMotor
        /// </summary>
        /// <param name="mainMotor"></param>
        /// <param name="fish"></param>
        /// <returns></returns>
        private bool AddFishToFreeAquarium(GameObject fish)
        {
            // We hit a supported fish with our SeaTruck cab. Iterate over all Aquarium modules and add the fish to
            // the first one with space
            SeaTruckAquarium[] seaTruckAquariums = _mainMotor.GetComponentsInChildren<SeaTruckAquarium>();
            SeaTruckFishScoopPlugin_BZ.Log.LogDebug($"Found {seaTruckAquariums.Length} aquarium modules");

            // Check to see if there are any aquariums
            if (seaTruckAquariums.Length == 0)
            {
                SeaTruckFishScoopPlugin_BZ.Log.LogDebug("No aquariums found.");
                return false;
            }

            foreach (SeaTruckAquarium seaTruckAquarium in seaTruckAquariums)
            {
                if (AddFishToAquarium(seaTruckAquarium, fish))
                {
                    string friendlyFishName = GetFriendlyName(fish.name);
                    SeaTruckFishScoopPlugin_BZ.Log.LogDebug($"Fish successfully added {fish.name} as {friendlyFishName}");
                    ErrorMessage.AddMessage($"Fish scoop successful! Added {friendlyFishName}");
                    return true;
                }
                else
                {
                    SeaTruckFishScoopPlugin_BZ.Log.LogDebug($"Unable to add fish to this aquarium ({seaTruckAquarium.name}). Likely full or fish is already in one.");
                }
            }
            SeaTruckFishScoopPlugin_BZ.Log.LogDebug("No free aquariums!");
            ErrorMessage.AddMessage($"Aquariums are full. Fish scoop failed!");
            return false;
        }

        /// <summary>
        /// Removes all fish from the specified Aquarium module
        /// </summary>
        /// <param name="seaTruckAquarium"></param>
        private static void PurgeFishFromAquarium(SeaTruckAquarium seaTruckAquarium)
        {
            // Get all creatures / aquariumfish
            ItemsContainer container = seaTruckAquarium.storageContainer.container;

            // Using ToSet() allows us to amend while iterating
            foreach (InventoryItem fishItem in container.ToSet())
            {
                // Release around the SeaTruck
                Pickupable fishPickupable = fishItem.item;
                Transform playerTransform = Player.main.transform;
                Vector3 fishPosition = playerTransform.position + (playerTransform.forward * 3.0f);
                SeaTruckFishScoopPlugin_BZ.Log.LogDebug($"Dropping fish at: {fishPosition}");
                fishPickupable.Drop(fishPosition);

                // Remove from aquarium container
                container.RemoveItem(fishPickupable, true);
                SeaTruckFishScoopPlugin_BZ.Log.LogDebug($"Removed {fishPickupable.name}");
            }
        }

        /// <summary>
        /// Returns a "user friendly" name for the fish caught
        /// </summary>
        /// <param name="fishName"></param>
        /// <returns></returns>
        private string GetFriendlyName(string fishName)
        {
            return (fishName.Replace("(Clone)", ""));
        }

        /// <summary>
        /// Add our fish to the chosen Aquarium
        /// </summary>
        /// <param name="seaTruckAquarium"></param>
        /// <param name="auquariumFish"></param>
        /// <returns></returns>
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
            else
            {
                return false;
            }
        }
    }
}